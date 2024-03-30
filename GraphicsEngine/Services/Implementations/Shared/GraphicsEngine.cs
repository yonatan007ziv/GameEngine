using GameEngine.Core.API;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Fonts;
using GameEngine.Core.Components.Input.Events;
using GameEngine.Core.Components.Objects;
using GameEngine.Core.SharedServices.Interfaces;
using GraphicsEngine.Components.Interfaces;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Numerics;

namespace GraphicsEngine.Services.Implementations.Shared;

internal class GraphicsEngine : IGraphicsEngine
{
	private readonly ILogger logger;
	private readonly IInternalGraphicsRenderer internalRenderer;
	private readonly IBufferDeletor bufferDeletor;
	private readonly IFileReader<Font> fontLoader;
	private readonly IFactory<ShaderSource, ShaderSource, IShaderProgram> shaderProgramFactory;
	private readonly IFactory<string, ShaderSource> shaderSourceFactory;
	public static GraphicsEngine EngineContext = null!;

	public IntPtr WindowHandle => internalRenderer.WindowHandle;
	public string Title { get => internalRenderer.Title; set => internalRenderer.Title = value; }
	public Vector2 WindowSize { get => internalRenderer.WindowSize; }
	public bool LogRenderingMessages { get => internalRenderer.LogRenderingMessages; set => internalRenderer.LogRenderingMessages = value; }

	private IShaderProgram textShader;
	private readonly Dictionary<string, Font> loadedFonts = new Dictionary<string, Font>();

	private readonly List<int> allObjectIds = new List<int>();

	private readonly Dictionary<int, RenderedWorldObject> worldObjects = new Dictionary<int, RenderedWorldObject>();
	private readonly Dictionary<int, RenderedUIObject> uiObjects = new Dictionary<int, RenderedUIObject>();
	private readonly Dictionary<int, RenderingWorldCamera> worldCameras = new Dictionary<int, RenderingWorldCamera>();
	private readonly Dictionary<int, RenderingUICamera> uiCameras = new Dictionary<int, RenderingUICamera>();

	// Buffer ids
	public List<int> FinalizedBuffers { get; } = new List<int>();
	public List<int> FinalizedVertexArrayBuffers { get; } = new List<int>();
	public List<int> FinalizedTextureBuffers { get; } = new List<int>();

	public event Action? Load;
	public event Action<MouseEventData>? MouseEvent;
	public event Action<KeyboardEventData>? KeyboardEvent;
	public event Action<GamepadEventData>? GamepadEvent;

	public GraphicsEngine(ILogger logger, IInternalGraphicsRenderer internalRenderer, IBufferDeletor bufferDeletor, IFileReader<Font> fontLoader, IFactory<ShaderSource, ShaderSource, IShaderProgram> shaderProgramFactory, IFactory<string, ShaderSource> shaderSourceFactory)
	{
		EngineContext = this;

		this.logger = logger;
		this.internalRenderer = internalRenderer;
		this.bufferDeletor = bufferDeletor;
		this.fontLoader = fontLoader;
		this.shaderProgramFactory = shaderProgramFactory;
		this.shaderSourceFactory = shaderSourceFactory;

		internalRenderer.LoadEvent += OnInternalEngineLoad;
		internalRenderer.ResizedEvent += WindowResized;
		internalRenderer.MouseEvent += (mouseEvent) => { MouseEvent?.Invoke(mouseEvent); };
		internalRenderer.KeyboardEvent += (keyboardEvent) => { KeyboardEvent?.Invoke(keyboardEvent); };
		internalRenderer.GamepadEvent += (joystickEvent) => { GamepadEvent?.Invoke(joystickEvent); };
	}

	private void OnInternalEngineLoad()
	{
		Load?.Invoke();

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
	}

	public void Run()
		=> internalRenderer.Start();

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

		internalRenderer.SetDepthTest(true);

		foreach (RenderingWorldCamera camera in worldCameras.Values)
		{
			camera.Update();
			internalRenderer.SetViewport(camera.ViewPort);

			// Render world objects
			foreach (RenderedWorldObject worldObject in worldObjects.Values)
			{
				if (!worldObject.WorldObject.Visible)
					continue;

				if (!camera.RenderingMask.InMask(worldObject.WorldObject.Tag)) // Exclude rendering mask
					worldObject.Render(camera);

				// Render objects' children
				foreach (RenderedWorldObject child in worldObject.Children)
					if (child.WorldObject.Visible)
						child.Render(camera);
			}
		}

		internalRenderer.SetDepthTest(false);

		foreach (RenderingUICamera camera in uiCameras.Values)
		{
			internalRenderer.SetViewport(camera.ViewPort);

			// Render UI objects' meshes
			foreach (RenderedUIObject uiObject in uiObjects.Values)
			{
				// Exclude invisible objects and rendering mask
				if (!uiObject.UIObject.Visible || camera.RenderingMask.InMask(uiObject.UIObject.Tag))
					continue;

				uiObject.Render(camera);
				foreach (RenderedUIObject child in uiObject.Children)
				{
					// Exclude invisible objects and rendering mask
					if (!child.UIObject.Visible || camera.RenderingMask.InMask(child.UIObject.Tag))
						continue;

					// Render meshes
					child.Render(camera);
				}
			}

			// Render UI objects' text
			textShader.Bind();
			foreach (RenderedUIObject uiObject in uiObjects.Values)
			{
				// Exclude invisible objects and rendering mask
				if (!uiObject.UIObject.Visible || camera.RenderingMask.InMask(uiObject.UIObject.Tag))
					continue;

				// Text boundary
				BoxData uiRect = new BoxData(uiObject.UIObject.Transform.Position, uiObject.UIObject.Transform.Scale);

				// Render text
				DrawText(uiObject.UIObject.TextData, uiRect);

				// Render object's children (text and meshes)
				foreach (RenderedUIObject child in uiObject.Children)
				{
					// Exclude invisible objects and rendering mask
					if (!child.UIObject.Visible || camera.RenderingMask.InMask(child.UIObject.Tag))
						continue;

					// Get relative bouding box to parent
					// In order to confine children elements inside the parent
					(Vector3 relativePosition, Vector3 relativeRotation, Vector3 relativeScale) = child.UIObject.Transform.GetRelativeTransform(uiObject.UIObject.Transform);
					BoxData childRect = new BoxData(relativePosition, relativeScale);

					// Render text
					DrawText(child.UIObject.TextData, childRect);
				}
			}
			textShader.Unbind();
		}

		internalRenderer.SwapBuffers();
	}

	public void DeleteFinalizedBuffers()
	{
		for (int i = 0; i < FinalizedBuffers.Count; i++)
			bufferDeletor.DeleteBuffer(FinalizedBuffers[i]);

		for (int i = 0; i < FinalizedVertexArrayBuffers.Count; i++)
			bufferDeletor.DeleteVertexArrayBuffer(FinalizedVertexArrayBuffers[i]);

		for (int i = 0; i < FinalizedTextureBuffers.Count; i++)
			bufferDeletor.DeleteTextureBuffer(FinalizedTextureBuffers[i]);

		FinalizedBuffers.Clear();
		FinalizedVertexArrayBuffers.Clear();
		FinalizedTextureBuffers.Clear();
	}

	private void DrawText(TextData textData, BoxData textArea)
	{
		string text = textData.Text;
		Color textColor = textData.TextColor;
		string fontName = textData.FontName;
		float fontSize = textData.FontSize;

		Font font;
		if (!loadedFonts.ContainsKey(fontName))
		{
			if (!fontLoader.ReadFile(fontName, out font))
			{
				logger.LogError("Font {fontName} could not be loaded", fontName);
				return;
			}

			loadedFonts[fontName] = font;
		}

		font = loadedFonts[fontName];
		font.FontSize = fontSize;

		// Check if all of the characters are supported
		foreach (char c in text)
			if (c != '\n' && c != ' ' && !font.CharacterMaps.ContainsKey(c))
			{
				logger.LogError("Text not supported by font");
				return;
			}

		string[] textLines = text.Split('\n');


		int longestLineLength = 0;
		// Compute total line width and height
		float textWidth = 0, linesHeight = 0;
		foreach (string line in textLines)
		{
			float currentWidth = 0;

			// Go through each letter, guaranteed to not be a newline due to split
			bool firstChar = true;
			foreach (char c in line)
			{
				if (firstChar)
					firstChar = false;
				else
					currentWidth += (c == ' ') ? font.CharacterMaps['L'].Width : font.CharacterMaps[c].Width;
			}

			if (textWidth < currentWidth)
			{
				textWidth = currentWidth;
				longestLineLength = line.Length;
			}

			// Get the tallest letter's height
			linesHeight += font.CharacterMaps['L'].Height;
		}

		// Shrink to fit
		float originalFontSize = font.FontSize;

		bool shrinkHorizontally = textWidth >= textArea.Scale.X * 2;
		bool shrinkVertically = linesHeight >= textArea.Scale.Y * 2;

		if (shrinkHorizontally || shrinkVertically)
		{
			// Compute maximum allowed font size
			float horizontalMaximumFont = textArea.Scale.X * 2 / longestLineLength;
			float verticalMaximumFont = textArea.Scale.Y * 2 / textLines.Length;

			// Meet both horizontal and vertical constraints
			font.FontSize = Math.Min(horizontalMaximumFont, verticalMaximumFont);

			// Recompute line metrics after shrinking
			textWidth = 0;
			linesHeight = 0;
			foreach (string line in textLines)
			{
				float currentWidth = 0;

				// Go through each letter, guaranteed to not be a newline due to split
				bool firstChar = true;
				foreach (char c in line)
				{
					if (firstChar)
						firstChar = false;
					else
						currentWidth += (c == ' ') ? font.CharacterMaps['L'].Width : font.CharacterMaps[c].Width;
				}

				if (textWidth < currentWidth)
					textWidth = currentWidth;

				// Get the tallest letter's height
				linesHeight += font.CharacterMaps['L'].Height;
			}
		}

		// Starting position is the centered position minus half of the total line length
		float x = textArea.Postion.X - textWidth / 2;
		float y = textArea.Postion.Y + (linesHeight - font.CharacterMaps['L'].Height) / 2;

		// Set the text color uniform
		textShader.SetFloat4Uniform(new Vector4((float)textColor.R / 0xFF, (float)textColor.G / 0xFF, (float)textColor.B / 0xFF, 1), "textColor");

		// Draw the text
		foreach (char c in text)
		{
			// Newline
			if (c == '\n')
			{
				// Reset the X coordinate
				x = textArea.Postion.X - textWidth / 2;

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

			CharacterGlyf currentGlyf = font.CharacterMaps[c];

			// Draw glyf
			internalRenderer.DrawGlyf(currentGlyf, new Vector2(x, y));

			x += currentGlyf.Width;
		}

		// Revert to original font size after shrinking to fit
		font.FontSize = originalFontSize;
	}

	private void WindowResized()
	{
		foreach (RenderingWorldCamera camera in worldCameras.Values)
		{
			camera.Width = WindowSize.X;
			camera.Height = WindowSize.Y;
		}
		foreach (RenderingUICamera camera in uiCameras.Values)
		{
			camera.Width = WindowSize.X;
			camera.Height = WindowSize.Y;
		}
	}

	#region Objects management
	#region World object
	public void AddWorldObject(WorldObject worldObject)
	{
		if (allObjectIds.Contains(worldObject.Id))
		{
			logger.LogError("Id already taken: {id}", worldObject.Id);
			return;
		}

		RenderedWorldObject renderedWorldObject = new RenderedWorldObject(worldObject, internalRenderer.MeshFactory);
		worldObjects.Add(worldObject.Id, renderedWorldObject);
		allObjectIds.Add(worldObject.Id);
	}

	public void AddWorldCamera(WorldObject camera, CameraRenderingMask<string> renderingMask, ViewPort viewPort)
	{
		if (camera.Parent is not null && !allObjectIds.Contains(camera.Parent.Id))
		{
			logger.LogError("Parent id not found: {id}", camera.Parent.Id);
			return;
		}
		if (allObjectIds.Contains(camera.Id))
		{
			logger.LogError("Id already taken: {id}", camera.Id);
			return;
		}

		RenderingWorldCamera renderingCamera = new RenderingWorldCamera(camera, renderingMask, (int)WindowSize.X, (int)WindowSize.Y, viewPort);

		worldCameras.Add(renderingCamera.CameraObject.Id, renderingCamera);
		allObjectIds.Add(renderingCamera.CameraObject.Id);
	}

	public void RemoveWorldObject(WorldObject worldObjectData)
	{
		if (!allObjectIds.Contains(worldObjectData.Id))
		{
			logger.LogError("World object id not found: {id}", worldObjectData.Id);
			return;
		}
		if (!worldObjects.ContainsKey(worldObjectData.Id))
		{
			logger.LogError("World object id not found: {id}", worldObjectData.Id);
			return;
		}

		worldObjects.Remove(worldObjectData.Id);
		allObjectIds.Remove(worldObjectData.Id);
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

		worldCameras.Remove(camera.Id);
		allObjectIds.Remove(camera.Id);
	}
	#endregion

	#region UI object
	public void AddUIObject(UIObject uiObject)
	{
		if (allObjectIds.Contains(uiObject.Id))
		{
			logger.LogError("Id already taken: {id}", uiObject.Id);
			return;
		}

		RenderedUIObject renderedUIObject = new RenderedUIObject(uiObject, internalRenderer.MeshFactory);
		uiObjects.Add(uiObject.Id, renderedUIObject);
		allObjectIds.Add(uiObject.Id);
	}

	public void AddUICamera(UIObject camera, CameraRenderingMask<string> renderingMask, ViewPort viewPort)
	{
		if (camera.Parent is not null && !allObjectIds.Contains(camera.Parent.Id))
		{
			logger.LogError("Parent id not found: {id}", camera.Parent.Id);
			return;
		}
		if (allObjectIds.Contains(camera.Id))
		{
			logger.LogError("Id already taken: {id}", camera.Id);
			return;
		}

		RenderingUICamera renderingCamera = new RenderingUICamera(camera, renderingMask, (int)WindowSize.X, (int)WindowSize.Y, viewPort);
		uiCameras.Add(renderingCamera.CameraObject.Id, renderingCamera);
		allObjectIds.Add(renderingCamera.CameraObject.Id);
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

		uiObjects.Remove(uiObject.Id);
		allObjectIds.Remove(uiObject.Id);
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

		uiCameras.Remove(camera.Id);
		allObjectIds.Remove(camera.Id);
	}
	#endregion
	#endregion
}