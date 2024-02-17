using GameEngine.Core.API;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Events;
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

	public IntPtr WindowHandle => internalRenderer.WindowHandle;
	public string Title { get => internalRenderer.Title; set => internalRenderer.Title = value; }
	public Vector2 WindowSize { get => internalRenderer.WindowSize; }
	public bool LogRenderingMessages { get => internalRenderer.LogRenderingMessages; set => internalRenderer.LogRenderingMessages = value; }

	private readonly List<int> allObjectIds = new List<int>();

	private readonly Dictionary<int, RenderedWorldObject> worldObjects = new Dictionary<int, RenderedWorldObject>();
	private readonly Dictionary<int, RenderedUIObject> uiObjects = new Dictionary<int, RenderedUIObject>();
	private readonly Dictionary<int, WorldCamera> worldCameras = new Dictionary<int, WorldCamera>();
	private readonly Dictionary<int, UICamera> uiCameras = new Dictionary<int, UICamera>();

	public event Action? Load;
	public event Action<MouseEventData>? MouseEvent;
	public event Action<KeyboardEventData>? KeyboardEvent;
	public event Action<GamepadEventData>? GamepadEvent;

	public GraphicsEngine(ILogger logger, IInternalGraphicsRenderer internalRenderer)
	{
		this.logger = logger;
		this.internalRenderer = internalRenderer;

		internalRenderer.LoadEvent += () => { Load?.Invoke(); };
		internalRenderer.ResizedEvent += WindowResized;
		internalRenderer.MouseEvent += (mouseEvent) => { MouseEvent?.Invoke(mouseEvent); };
		internalRenderer.KeyboardEvent += (keyboardEvent) => { KeyboardEvent?.Invoke(keyboardEvent); };
		internalRenderer.GamepadEvent += (joystickEvent) => { GamepadEvent?.Invoke(joystickEvent); };
	}

	public void Start()
		=> internalRenderer.Start();

	public void SetBackgroundColor(Color color)
		=> internalRenderer.SetBackgroundColor(color);

	public void LockMouse(bool lockMouse)
	{
		internalRenderer.SetMouseLocked(lockMouse);
		internalRenderer.SetMousePosition(WindowSize / 2);
	}

	public void RenderFrame()
	{
		internalRenderer.ProcessEvents();

		internalRenderer.SetDepthTest(true);

		WorldCamera[] worldCameras = this.worldCameras.Values.ToArray();
		RenderedWorldObject[] worldObjects = this.worldObjects.Values.ToArray();

		foreach (WorldCamera camera in worldCameras)
		{
			camera.Update();
			internalRenderer.SetViewport(camera.ViewPort);

			// Render world objects
			foreach (RenderedWorldObject worldObject in worldObjects)
				if (worldObject.Id != camera.ParentId)
					worldObject.Render(camera);
		}

		internalRenderer.SetDepthTest(false);

		UICamera[] uiCameras = this.uiCameras.Values.ToArray();
		RenderedUIObject[] uiObjects = this.uiObjects.Values.ToArray();

		foreach (UICamera camera in uiCameras)
		{
			internalRenderer.SetViewport(camera.ViewPort);

			// Render UI Objects
			foreach (RenderedUIObject uiObject in uiObjects)
				uiObject.Render(camera);
		}

		internalRenderer.SwapBuffers();
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
	public void UpdateWorldObject(ref WorldObjectData worldObjectData)
	{
		if (!allObjectIds.Contains(worldObjectData.Id))
		{
			logger.LogError("GraphicsEngine. Object id not found: {id}", worldObjectData.Id);
			return;
		}
		if (!worldObjects.ContainsKey(worldObjectData.Id))
		{
			logger.LogError("GraphicsEngine. World object id not found: {id}", worldObjectData.Id);
			return;
		}

		RenderedWorldObject worldObject = worldObjects[worldObjectData.Id];

		if (worldObjectData.TransformDirty)
			worldObject.Transform.CopyFrom(worldObjectData.Transform);


		if (worldObjectData.MeshesDirty)
		{
			worldObject.Meshes.Clear();
			for (int i = 0; i < worldObjectData.Meshes.Count; i++)
				if (internalRenderer.MeshFactory.Create(worldObjectData.Meshes[i].Model, worldObjectData.Meshes[i].Material, out IMeshRenderer meshRenderer))
					worldObject.Meshes.Add(meshRenderer);
				else
					logger.LogError("Error creating MeshRenderer: {model}, {material}", worldObjectData.Meshes[i].Model, worldObjectData.Meshes[i].Material);
		}

		worldObject.Update();
	}
	public void UpdateUIObject(ref UIObjectData uiObjectData)
	{
		if (!allObjectIds.Contains(uiObjectData.Id))
		{
			logger.LogError("Id not found: {id}", uiObjectData.Id);
			return;
		}
		if (!uiObjects.ContainsKey(uiObjectData.Id))
		{
			logger.LogError("UI object id not found: {id}", uiObjectData.Id);
			return;
		}

		RenderedUIObject uiObject = uiObjects[uiObjectData.Id];

		if (uiObjectData.TransformDirty)
			uiObject.Transform.CopyFrom(uiObjectData.Transform);

		if (uiObjectData.MeshesDirty)
		{
			uiObject.Meshes.Clear();
			for (int i = 0; i < uiObjectData.Meshes.Count; i++)
				if (internalRenderer.MeshFactory.Create(uiObjectData.Meshes[i].Model, uiObjectData.Meshes[i].Material, out IMeshRenderer meshRenderer))
					uiObject.Meshes.Add(meshRenderer);
				else
					logger.LogError("Error creating MeshRenderer: {model}, {material}", uiObjectData.Meshes[i].Model, uiObjectData.Meshes[i].Material);
		}

		uiObject.Update();
	}

	#region Add object
	public void AddWorldObject(ref WorldObjectData worldObjectData)
	{
		if (allObjectIds.Contains(worldObjectData.Id))
		{
			logger.LogError("Id already taken: {id}", worldObjectData.Id);
			return;
		}

		List<IMeshRenderer> meshes = new List<IMeshRenderer>();
		for (int i = 0; i < worldObjectData.Meshes.Count; i++)
			if (internalRenderer.MeshFactory.Create(worldObjectData.Meshes[i].Model, worldObjectData.Meshes[i].Material, out IMeshRenderer meshRenderer))
				meshes.Add(meshRenderer);
			else
				logger.LogError("Error creating MeshRenderer: {model}, {material}", worldObjectData.Meshes[i].Model, worldObjectData.Meshes[i].Material);

		RenderedWorldObject worldObject = new RenderedWorldObject(worldObjectData.Id, new Transform(worldObjectData.Transform), meshes.ToArray());
		worldObjects.Add(worldObjectData.Id, worldObject);
		allObjectIds.Add(worldObjectData.Id);
	}
	public void AddWorldCamera(ref GameComponentData cameraData, ViewPort viewPort)
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
	public void AddUIObject(ref UIObjectData uiObjectData)
	{
		if (allObjectIds.Contains(uiObjectData.Id))
		{
			logger.LogError("Id already taken: {id}", uiObjectData.Id);
			return;
		}

		List<IMeshRenderer> meshes = new List<IMeshRenderer>();
		for (int i = 0; i < uiObjectData.Meshes.Count; i++)
			if (internalRenderer.MeshFactory.Create(uiObjectData.Meshes[i].Model, uiObjectData.Meshes[i].Material, out IMeshRenderer meshRenderer))
				meshes.Add(meshRenderer);
			else
				logger.LogError("Error creating MeshRenderer: {model}, {material}", uiObjectData.Meshes[i].Model, uiObjectData.Meshes[i].Material);

		RenderedUIObject uiObject = new RenderedUIObject(uiObjectData.Id, new Transform(uiObjectData.Transform), meshes.ToArray());
		uiObjects.Add(uiObjectData.Id, uiObject);
		allObjectIds.Add(uiObjectData.Id);
	}
	public void AddUICamera(ref GameComponentData cameraData, ViewPort viewPort)
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
	#endregion

	#region Remove object
	public void RemoveWorldObject(ref WorldObjectData worldObjectData)
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
	public void RemoveWorldCamera(ref GameComponentData cameraData)
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
	public void RemoveUIObject(ref UIObjectData uiObjectData)
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
	public void RemoveUICamera(ref GameComponentData cameraData)
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