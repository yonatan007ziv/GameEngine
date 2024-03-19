using GameEngine.Core.API;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Fonts;
using GameEngine.Core.Components.Input.Events;
using GameEngine.Core.Components.Objects;
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

	public static GraphicsEngine EngineContext = null!;

	public IntPtr WindowHandle => internalRenderer.WindowHandle;
	public string Title { get => internalRenderer.Title; set => internalRenderer.Title = value; }
	public Vector2 WindowSize { get => internalRenderer.WindowSize; }
	public bool LogRenderingMessages { get => internalRenderer.LogRenderingMessages; set => internalRenderer.LogRenderingMessages = value; }

	private readonly List<int> allObjectIds = new List<int>();

	private readonly Dictionary<int, RenderedWorldObject> worldObjects = new Dictionary<int, RenderedWorldObject>();
	private readonly Dictionary<int, RenderedUIObject> uiObjects = new Dictionary<int, RenderedUIObject>();
	private readonly Dictionary<int, WorldCamera> worldCameras = new Dictionary<int, WorldCamera>();
	private readonly Dictionary<int, UICamera> uiCameras = new Dictionary<int, UICamera>();

	// Buffer ids
	public List<int> FinalizedBuffers { get; } = new List<int>();
	public List<int> FinalizedVertexArrayBuffers { get; } = new List<int>();
	public List<int> FinalizedTextureBuffers { get; } = new List<int>();

	public event Action? Load;
	public event Action<MouseEventData>? MouseEvent;
	public event Action<KeyboardEventData>? KeyboardEvent;
	public event Action<GamepadEventData>? GamepadEvent;

	public GraphicsEngine(ILogger logger, IInternalGraphicsRenderer internalRenderer, IBufferDeletor bufferDeletor)
	{
		EngineContext = this;

		this.logger = logger;
		this.internalRenderer = internalRenderer;
		this.bufferDeletor = bufferDeletor;

		internalRenderer.LoadEvent += () => { Load?.Invoke(); };
		internalRenderer.ResizedEvent += WindowResized;
		internalRenderer.MouseEvent += (mouseEvent) => { MouseEvent?.Invoke(mouseEvent); };
		internalRenderer.KeyboardEvent += (keyboardEvent) => { KeyboardEvent?.Invoke(keyboardEvent); };
		internalRenderer.GamepadEvent += (joystickEvent) => { GamepadEvent?.Invoke(joystickEvent); };
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

		foreach (WorldCamera camera in worldCameras.Values)
		{
			camera.Update();
			internalRenderer.SetViewport(camera.ViewPort);

			// Render world objects
			foreach (RenderedWorldObject worldObject in worldObjects.Values)
				if (worldObject.Id != camera.ParentId)
					worldObject.Render(camera);
		}

		internalRenderer.SetDepthTest(false);

		foreach (UICamera camera in uiCameras.Values)
		{
			internalRenderer.SetViewport(camera.ViewPort);

			// Render UI Objects
			foreach (RenderedUIObject uiObject in uiObjects.Values)
			{
				uiObject.Render(camera);

				// Render text
				//int offsetX = 0, offsetY = 0;
				//foreach (char c in uiObject.TextData.Text)
				//{
				//	CharacterGlyf currentGlyf = uiObject.TextData.Font.CharacterMaps[c];
				//
				//	// Draw
				//	DrawCharacterGlyf(currentGlyf, uiObject.Transform.Position + new Vector3(offsetX, offsetY, 0));
				//
				//	offsetX += currentGlyf.Width;
				//	// If newline
				//	if (c == '\n')
				//	{
				//		offsetX = 0;
				//		offsetY += currentGlyf.Height;
				//	}
				//}
			}
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

	private void DrawCharacterGlyf(CharacterGlyf glyf, Vector3 position)
	{

	}

	private void WindowResized()
	{
		foreach (WorldCamera camera in worldCameras.Values)
		{
			camera.Width = WindowSize.X;
			camera.Height = WindowSize.Y;
		}
		foreach (UICamera camera in uiCameras.Values)
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

	public void AddWorldCamera(GameComponentData cameraData, ViewPort viewPort)
	{
		if (!allObjectIds.Contains(cameraData.ParentId))
		{
			logger.LogError("Parent id not found: {id}", cameraData.ParentId);
			return;
		}
		if (allObjectIds.Contains(cameraData.Id))
		{
			logger.LogError("Id already taken: {id}", cameraData.Id);
			return;
		}

		// Find parent
		RenderedWorldObject parent = worldObjects[cameraData.ParentId];
		WorldCamera camera = new WorldCamera(cameraData.Id, cameraData.ParentId, parent.Transform, (int)WindowSize.X, (int)WindowSize.Y, viewPort);
		worldCameras.Add(cameraData.Id, camera);
		allObjectIds.Add(cameraData.Id);
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

	public void RemoveWorldCamera(GameComponentData cameraData)
	{
		if (!allObjectIds.Contains(cameraData.Id))
		{
			logger.LogError("Id not found: {id}", cameraData.Id);
			return;
		}
		if (!worldCameras.ContainsKey(cameraData.Id))
		{
			logger.LogError("World camera id not found: {id}", cameraData.Id);
			return;
		}

		worldCameras.Remove(cameraData.Id);
		allObjectIds.Remove(cameraData.Id);
	}
	#endregion

	#region UI object
	public void AddUIObject(UIObject uiObjectData)
	{
		if (allObjectIds.Contains(uiObjectData.Id))
		{
			logger.LogError("Id already taken: {id}", uiObjectData.Id);
			return;
		}

		RenderedUIObject uiObject = new RenderedUIObject(uiObjectData, internalRenderer.MeshFactory);
		uiObjects.Add(uiObjectData.Id, uiObject);
		allObjectIds.Add(uiObjectData.Id);
	}

	public void AddUICamera(GameComponentData cameraData, ViewPort viewPort)
	{
		if (!allObjectIds.Contains(cameraData.ParentId))
		{
			logger.LogError("Parent id not found: {id}", cameraData.ParentId);
			return;
		}
		if (allObjectIds.Contains(cameraData.Id))
		{
			logger.LogError("Id already taken: {id}", cameraData.Id);
			return;
		}

		// Find parent
		UICamera camera = new UICamera(cameraData.Id, (int)WindowSize.X, (int)WindowSize.Y, viewPort);
		uiCameras.Add(cameraData.Id, camera);
		allObjectIds.Add(cameraData.Id);
	}

	public void RemoveUIObject(UIObject uiObjectData)
	{
		if (!allObjectIds.Contains(uiObjectData.Id))
		{
			logger.LogError("UI object id not found: {id}", uiObjectData.Id);
			return;
		}
		if (!uiObjects.ContainsKey(uiObjectData.Id))
		{
			logger.LogError("UI object id not found: {id}", uiObjectData.Id);
			return;
		}

		uiObjects.Remove(uiObjectData.Id);
		allObjectIds.Remove(uiObjectData.Id);
	}

	public void RemoveUICamera(GameComponentData cameraData)
	{
		if (!allObjectIds.Contains(cameraData.Id))
		{
			logger.LogError("Id not found: {id}", cameraData.Id);
			return;
		}
		if (!uiCameras.ContainsKey(cameraData.Id))
		{
			logger.LogError("UI camera id not found: {id}", cameraData.Id);
			return;
		}

		uiCameras.Remove(cameraData.Id);
		allObjectIds.Remove(cameraData.Id);
	}
	#endregion
	#endregion
}