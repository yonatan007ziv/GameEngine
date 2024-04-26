using GameEngine.Core.API;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Fonts;
using GameEngine.Core.Components.Input.Events;
using GameEngine.Core.Components.Objects;
using GameEngine.Core.SharedServices.Interfaces;
using GraphicsEngine.Components.Interfaces;
using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Components.Shared.Data;
using GraphicsEngine.Components.Shared.RenderedObjects;
using GraphicsEngine.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Numerics;

namespace GraphicsEngine.Services.Implementations.Shared;

internal class GraphicsEngine : IGraphicsEngine
{
	private readonly ILogger logger;
	private readonly IInternalGraphicsRenderer internalRenderer;
	private readonly IBufferFactory bufferFactory;
	private readonly IBufferDeletor bufferDeletor;
	private readonly IFileReader<Font> fontLoader;
	private readonly IFactory<ShaderSource, ShaderSource, IShaderProgram> shaderProgramFactory;
	private readonly IFactory<string, ShaderSource> shaderSourceFactory;
	public static GraphicsEngine EngineContext = null!;

	public IntPtr WindowHandle => internalRenderer.WindowHandle;
	public string Title { get => internalRenderer.Title; set => internalRenderer.Title = value; }
	public Vector2 WindowSize { get => internalRenderer.WindowSize; }
	public bool LogRenderingMessages { get => internalRenderer.LogRenderingMessages; set => internalRenderer.LogRenderingMessages = value; }

	private IVertexBuffer textVertexBuffer;
	private IIndexBuffer textIndexBuffer;
	private IVertexArray textVertexArray;
	private IShaderProgram textShader;
	private readonly Dictionary<string, Font> loadedFonts = new Dictionary<string, Font>();
	private readonly List<(CharacterGlyf glyph, Vector2 position, float fontSize, Color textColor)> frameCharacterGlyphs = new List<(CharacterGlyf, Vector2, float, Color textColor)>();

	private readonly List<int> allObjectIds = new List<int>();

	private readonly Dictionary<int, RenderedWorldObject> worldObjects = new Dictionary<int, RenderedWorldObject>();
	private readonly Dictionary<int, RenderedUIObject> uiObjects = new Dictionary<int, RenderedUIObject>();
	private readonly Dictionary<int, WorldRenderingCamera> worldCameras = new Dictionary<int, WorldRenderingCamera>();
	private readonly Dictionary<int, UIRenderingCamera> uiCameras = new Dictionary<int, UIRenderingCamera>();

	// Finalized buffer ids
	public Queue<int> FinalizedBuffers { get; } = new Queue<int>();
	public Queue<int> FinalizedVertexArrayBuffers { get; } = new Queue<int>();
	public Queue<int> FinalizedTextureBuffers { get; } = new Queue<int>();

	public event Action? Load;
	public event Action<MouseEventData>? MouseEvent;
	public event Action<KeyboardEventData>? KeyboardEvent;
	public event Action<GamepadEventData>? GamepadEvent;
	public Action<Vector2> ScreenSizeChanged { get; set; }

	public GraphicsEngine(ILogger logger, IInternalGraphicsRenderer internalRenderer, IBufferFactory bufferFactory, IBufferDeletor bufferDeletor, IFileReader<Font> fontLoader, IFactory<ShaderSource, ShaderSource, IShaderProgram> shaderProgramFactory, IFactory<string, ShaderSource> shaderSourceFactory)
	{
		EngineContext = this;

		this.logger = logger;
		this.internalRenderer = internalRenderer;
		this.bufferFactory = bufferFactory;
		this.bufferDeletor = bufferDeletor;
		this.fontLoader = fontLoader;
		this.shaderProgramFactory = shaderProgramFactory;
		this.shaderSourceFactory = shaderSourceFactory;

		internalRenderer.LoadEvent += OnInternalEngineLoad;
		internalRenderer.ResizedEvent += WindowResized;
		internalRenderer.MouseEvent += (mouseEvent) => MouseEvent?.Invoke(mouseEvent);
		internalRenderer.KeyboardEvent += (keyboardEvent) => KeyboardEvent?.Invoke(keyboardEvent);
		internalRenderer.GamepadEvent += (joystickEvent) => GamepadEvent?.Invoke(joystickEvent);

		textVertexBuffer = null!;
		textIndexBuffer = null!;
		textVertexArray = null!;
		textShader = null!;
	}

	public void Run()
		=> internalRenderer.Start();

	private void OnInternalEngineLoad()
	{
		Load?.Invoke();
		CreateTextGraphics();
	}

	public void SetBackgroundColor(Color color)
		=> internalRenderer.SetBackgroundColor(color);

	public void SetLockedMouse(bool lockMouse)
	{
		internalRenderer.SetMouseLocked(lockMouse);
		internalRenderer.SetMousePosition(WindowSize / 2);
	}

	public void RenderFrame()
	{
		internalRenderer.ProcessEvents();

		// Go through update queues
		UpdateObjects();

		internalRenderer.SetDepthTest(true);
		for (int i = 0; i < worldCameras.Count; i++)
			for (int j = 0; j < worldObjects.Count; j++)
				RenderWorldObject(worldCameras.Values.ElementAt(i), worldObjects.Values.ElementAt(j));

		internalRenderer.SetDepthTest(false);
		for (int i = 0; i < uiCameras.Count; i++)
			for (int j = 0; j < uiObjects.Count; j++)
				RenderUIObject(uiCameras.Values.ElementAt(i), uiObjects.Values.ElementAt(j));

		// Render "buffered" text
		textShader.Bind();
		internalRenderer.DrawGlyphs(frameCharacterGlyphs, textVertexBuffer, textIndexBuffer, textVertexArray, textShader);
		textShader.Unbind();
		frameCharacterGlyphs.Clear();

		internalRenderer.SwapBuffers();
	}

	private void UpdateObjects()
	{
		// World objects updates
		while (_addWorldObjectsQueue.Count > 0)
			InternalAddWorldObject(_addWorldObjectsQueue.Dequeue());
		while (_removeWorldObjectsQueue.Count > 0)
			InternalRemoveWorldObject(_removeWorldObjectsQueue.Dequeue());
		while (_addWorldCamerasQueue.Count > 0)
			InternalAddWorldCamera(_addWorldCamerasQueue.Dequeue());
		while (_removeWorldCamerasQueue.Count > 0)
			InternalRemoveWorldCamera(_removeWorldCamerasQueue.Dequeue());

		// UI objects updates
		while (_addUIObjectsQueue.Count > 0)
			InternalAddUIObject(_addUIObjectsQueue.Dequeue());
		while (_removeUIObjectsQueue.Count > 0)
			InternalRemoveUIObject(_removeUIObjectsQueue.Dequeue());
		while (_addUICamerasQueue.Count > 0)
			InternalAddUICamera(_addUICamerasQueue.Dequeue());
		while (_removeUICamerasQueue.Count > 0)
			InternalRemoveUICamera(_removeUICamerasQueue.Dequeue());
	}

	private void RenderWorldObject(WorldRenderingCamera camera, RenderedWorldObject worldObject)
	{
		internalRenderer.SetViewport(camera.ViewPort);

		camera.Update();

		// Exclude invisible objects and rendering mask
		if (!worldObject.WorldObject.Visible || camera.RenderingMask.MaskContains(worldObject.WorldObject.Tag))
			return;

		worldObject.Render(camera);

		// Render objects' children
		for (int i = 0; i < worldObject.Children.Count; i++)
			if (worldObject.Children[i] is RenderedWorldObject worldChild)
				RenderWorldObject(camera, worldChild);
	}

	private void RenderUIObject(UIRenderingCamera camera, RenderedUIObject uiObject)
	{
		internalRenderer.SetViewport(camera.ViewPort);

		// Exclude invisible objects and rendering mask
		if (!uiObject.UIObject.Visible || camera.RenderingMask.MaskContains(uiObject.UIObject.Tag))
			return;

		// Render UI objects' meshes
		uiObject.Render(camera);

		// // Render UI objects' text
		(Vector3 position, Vector3 rotation, Vector3 scale) relativeAncestorTransform = uiObject.UIObject.GetRelativeToAncestorTransform();
		textShader.Bind();
		BufferText(uiObject.UIObject.TextData, (relativeAncestorTransform.position, relativeAncestorTransform.scale));
		textShader.Unbind();

		// Render objects' children
		for (int i = 0; i < uiObject.Children.Count; i++)
			if (uiObject.Children[i] is RenderedUIObject uiChild)
				RenderUIObject(camera, uiChild);
	}

	public void DeleteFinalizedBuffers()
	{
		while (FinalizedBuffers.Count > 0)
			bufferDeletor.DeleteBuffer(FinalizedBuffers.Dequeue());
		while (FinalizedVertexArrayBuffers.Count > 0)
			bufferDeletor.DeleteVertexArrayBuffer(FinalizedVertexArrayBuffers.Dequeue());
		while (FinalizedTextureBuffers.Count > 0)
			bufferDeletor.DeleteTextureBuffer(FinalizedTextureBuffers.Dequeue());
	}

	private void BufferText(TextData textData, (Vector3 position, Vector3 scale) textArea)
	{
		Font font;
		if (!loadedFonts.ContainsKey(textData.FontName))
		{
			if (!fontLoader.ReadFile(textData.FontName, out font))
			{
				logger.LogError("Font {fontName} could not be loaded", textData.FontName);
				return;
			}

			loadedFonts[textData.FontName] = font;
		}

		font = loadedFonts[textData.FontName];

		// Check if all of the characters are supported
		foreach (char c in textData.Text)
			if (c != '\n' && c != ' ' && !font.CharacterMaps.ContainsKey(c))
			{
				logger.LogError("Text not supported by font");
				return;
			}

		string[] textLines = textData.Text.Split('\n');

		int longestLineLength = (textLines.OrderByDescending(s => s.Length).FirstOrDefault() ?? "").Length;
		// Compute total line width and height
		float textWidth = textData.FontSize * longestLineLength - font.FontSize;
		float linesHeight = textData.FontSize * textLines.Length;

		// Shrink to fit
		bool shrinkHorizontally = textWidth >= textArea.scale.X * 2;
		bool shrinkVertically = linesHeight >= textArea.scale.Y * 2;

		if (shrinkHorizontally || shrinkVertically)
		{
			// Compute maximum allowed font size
			float horizontalMaximumFont = textArea.scale.X * 2 / longestLineLength;
			float verticalMaximumFont = textArea.scale.Y * 2 / textLines.Length;

			// Meet both horizontal and vertical constraints
			font.FontSize = Math.Min(horizontalMaximumFont, verticalMaximumFont);

			// Recompute line metrics after shrinking
			textWidth = font.FontSize * longestLineLength - font.FontSize;
			linesHeight = font.FontSize * textLines.Length;
		}
		else
			font.FontSize = textData.FontSize;

		// Starting position is the centered position minus half of the total line length
		float x = textArea.position.X - textWidth / 2;
		float y = textArea.position.Y + (linesHeight - font.CharacterMaps['L'].Height) / 2;

		// Draw the text
		foreach (char c in textData.Text)
		{
			// Newline
			if (c == '\n')
			{
				// Reset the X coordinate
				x = textArea.position.X - textWidth / 2;

				// Get the tallest letter's height and subtract it from the y coordinate
				y -= font.CharacterMaps['L'].Height;

				continue;
			}

			// Space
			if (c == ' ')
			{
				// Get the widest letter's width and add it to the x coordinate
				x += font.CharacterMaps['L'].Width;

				continue;
			}

			CharacterGlyf currentGlyph = font.CharacterMaps[c];
			frameCharacterGlyphs.Add((currentGlyph, new Vector2(x, y), font.FontSize, textData.TextColor));
			x += currentGlyph.Width;
		}
	}

	private void CreateTextGraphics()
	{
		// Create text shader program
		if (!shaderSourceFactory.Create("TextVertex.glsl", out ShaderSource textVertexSource))
		{
			logger.LogCritical("Text shader not created successfully");
			return;
		}

		if (!shaderSourceFactory.Create("TextFragment.glsl", out ShaderSource textFragmentSource))
		{
			logger.LogCritical("Text shader not created successfully");
			return;
		}

		if (!shaderProgramFactory.Create(textVertexSource, textFragmentSource, out textShader))
		{
			logger.LogCritical("Text shader not created successfully");
			return;
		}

		textVertexBuffer = bufferFactory.GenerateVertexBuffer();
		textIndexBuffer = bufferFactory.GenerateIndexBuffer();
		textVertexArray = bufferFactory.GenerateVertexArray(textVertexBuffer, textIndexBuffer, [new AttributeLayout(typeof(float), 2)]);
	}

	private void WindowResized()
	{
		for (int i = 0; i < worldCameras.Count; i++)
		{
			worldCameras.Values.ElementAt(i).Width = WindowSize.X;
			worldCameras.Values.ElementAt(i).Height = WindowSize.Y;
		}
		for (int i = 0; i < uiCameras.Count; i++)
		{
			uiCameras.Values.ElementAt(i).Width = WindowSize.X;
			uiCameras.Values.ElementAt(i).Height = WindowSize.Y;
		}
		ScreenSizeChanged?.Invoke(internalRenderer.WindowSize);
	}

	#region Objects management
	#region World object
	private readonly Queue<WorldObject> _addWorldObjectsQueue = new Queue<WorldObject>();
	private readonly Queue<WorldObject> _removeWorldObjectsQueue = new Queue<WorldObject>();
	private readonly Queue<(WorldObject camera, CameraRenderingMask renderingMask, ViewPort viewPort)> _addWorldCamerasQueue = new Queue<(WorldObject camera, CameraRenderingMask renderingMask, ViewPort viewPort)>();
	private readonly Queue<WorldObject> _removeWorldCamerasQueue = new Queue<WorldObject>();
	#region Internal object management
	private void InternalAddWorldObject(WorldObject worldObject)
	{
		RenderedWorldObject renderedWorldObject = new RenderedWorldObject(worldObject, internalRenderer.MeshFactory);
		worldObjects.Add(worldObject.Id, renderedWorldObject);
		allObjectIds.Add(worldObject.Id);
	}
	private void InternalRemoveWorldObject(WorldObject worldObject)
	{
		worldObjects.Remove(worldObject.Id);
		allObjectIds.Remove(worldObject.Id);
	}
	private void InternalAddWorldCamera((WorldObject camera, CameraRenderingMask renderingMask, ViewPort viewPort) cameraTuple)
	{
		WorldRenderingCamera renderingCamera = new WorldRenderingCamera(cameraTuple.camera, cameraTuple.renderingMask, (int)WindowSize.X, (int)WindowSize.Y, cameraTuple.viewPort);
		worldCameras.Add(renderingCamera.CameraObject.Id, renderingCamera);
		allObjectIds.Add(renderingCamera.CameraObject.Id);
	}
	private void InternalRemoveWorldCamera(WorldObject camera)
	{
		worldCameras.Remove(camera.Id);
		allObjectIds.Remove(camera.Id);
	}
	#endregion

	public void AddWorldObject(WorldObject worldObject)
	{
		if (allObjectIds.Contains(worldObject.Id))
		{
			logger.LogError("Id already taken: {id}", worldObject.Id);
			return;
		}

		_addWorldObjectsQueue.Enqueue(worldObject);
	}
	public void RemoveWorldObject(WorldObject worldObject)
	{
		if (!allObjectIds.Contains(worldObject.Id))
		{
			logger.LogError("World object id not found: {id}", worldObject.Id);
			return;
		}
		if (!worldObjects.ContainsKey(worldObject.Id))
		{
			logger.LogError("World object id not found: {id}", worldObject.Id);
			return;
		}

		_removeWorldObjectsQueue.Enqueue(worldObject);
	}
	public void AddWorldCamera(WorldObject camera, CameraRenderingMask renderingMask, ViewPort viewPort)
	{
		if (camera.Parent is not null && !(allObjectIds.Contains(camera.Parent.Id) || _addWorldObjectsQueue.Contains(camera.Parent)))
		{
			logger.LogError("Parent id not found: {id}", camera.Parent.Id);
			return;
		}
		if (allObjectIds.Contains(camera.Id))
		{
			logger.LogError("Id already taken: {id}", camera.Id);
			return;
		}

		_addWorldCamerasQueue.Enqueue((camera, renderingMask, viewPort));
	}
	public void RemoveWorldCamera(WorldObject camera)
	{
		if (!allObjectIds.Contains(camera.Id))
		{
			logger.LogError("Id not found: {id}", camera.Id);
			return;
		}
		if (!worldCameras.ContainsKey(camera.Id))
		{
			logger.LogError("World camera id not found: {id}", camera.Id);
			return;
		}

		_removeWorldCamerasQueue.Enqueue(camera);
	}
	#endregion
	#region UI object
	private readonly Queue<UIObject> _addUIObjectsQueue = new Queue<UIObject>();
	private readonly Queue<UIObject> _removeUIObjectsQueue = new Queue<UIObject>();
	private readonly Queue<(UIObject camera, CameraRenderingMask renderingMask, ViewPort viewPort)> _addUICamerasQueue = new Queue<(UIObject camera, CameraRenderingMask renderingMask, ViewPort viewPort)>();
	private readonly Queue<UIObject> _removeUICamerasQueue = new Queue<UIObject>();
	#region Internal object management
	private void InternalAddUIObject(UIObject uiObject)
	{
		RenderedUIObject renderedUIObject = new RenderedUIObject(uiObject, internalRenderer.MeshFactory);
		uiObjects.Add(uiObject.Id, renderedUIObject);
		allObjectIds.Add(uiObject.Id);
	}
	private void InternalRemoveUIObject(UIObject uiObject)
	{
		uiObjects.Remove(uiObject.Id);
		allObjectIds.Remove(uiObject.Id);
	}
	private void InternalAddUICamera((UIObject camera, CameraRenderingMask renderingMask, ViewPort viewPort) cameraTuple)
	{
		UIRenderingCamera renderingCamera = new UIRenderingCamera(cameraTuple.camera, cameraTuple.renderingMask, (int)WindowSize.X, (int)WindowSize.Y, cameraTuple.viewPort);
		uiCameras.Add(renderingCamera.CameraObject.Id, renderingCamera);
		allObjectIds.Add(renderingCamera.CameraObject.Id);
	}
	private void InternalRemoveUICamera(UIObject camera)
	{
		uiCameras.Remove(camera.Id);
		allObjectIds.Remove(camera.Id);
	}
	#endregion

	public void AddUIObject(UIObject uiObject)
	{
		if (allObjectIds.Contains(uiObject.Id))
		{
			logger.LogError("Id already taken: {id}", uiObject.Id);
			return;
		}

		_addUIObjectsQueue.Enqueue(uiObject);
	}
	public void RemoveUIObject(UIObject uiObject)
	{
		if (!allObjectIds.Contains(uiObject.Id))
		{
			logger.LogError("UI object id not found: {id}", uiObject.Id);
			return;
		}
		if (!uiObjects.ContainsKey(uiObject.Id))
		{
			logger.LogError("UI object id not found: {id}", uiObject.Id);
			return;
		}

		_removeUIObjectsQueue.Enqueue(uiObject);
	}
	public void AddUICamera(UIObject camera, CameraRenderingMask renderingMask, ViewPort viewPort)
	{
		if (camera.Parent is not null && !(allObjectIds.Contains(camera.Parent.Id) || _addUIObjectsQueue.Contains(camera.Parent)))
		{
			logger.LogError("Parent id not found: {id}", camera.Parent.Id);
			return;
		}
		if (allObjectIds.Contains(camera.Id))
		{
			logger.LogError("Id already taken: {id}", camera.Id);
			return;
		}

		_addUICamerasQueue.Enqueue((camera, renderingMask, viewPort));
	}
	public void RemoveUICamera(UIObject camera)
	{
		if (!allObjectIds.Contains(camera.Id))
		{
			logger.LogError("Id not found: {id}", camera.Id);
			return;
		}
		if (!uiCameras.ContainsKey(camera.Id))
		{
			logger.LogError("UI camera id not found: {id}", camera.Id);
			return;
		}

		_removeUICamerasQueue.Enqueue(camera);
	}
	#endregion
	#endregion
}