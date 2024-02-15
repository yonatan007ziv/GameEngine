using GameEngine.Core.API;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Events;
using GameEngine.Core.Extensions;
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

	private readonly Dictionary<int, IObject> allObjects = new Dictionary<int, IObject>();
	private readonly Dictionary<int, Camera> worldCameras = new Dictionary<int, Camera>();
	private readonly Dictionary<int, Camera> uiCameras = new Dictionary<int, Camera>();
	private readonly Dictionary<int, RenderedObject> worldObjects = new Dictionary<int, RenderedObject>();
	private readonly Dictionary<int, RenderedObject> uiObjects = new Dictionary<int, RenderedObject>();

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
		=> internalRenderer.SetMouseLocked(lockMouse);

	public void RenderFrame()
	{
		internalRenderer.ProcessEvents();

		internalRenderer.SetDepthTest(true);

		foreach (Camera camera in worldCameras.Values)
		{
			camera.Transform.Rotation += new Vector3(0, 0.2f, 0);
			camera.Update();
			internalRenderer.SetViewport(camera.ViewPort);

			// Render world objects
			foreach (RenderedObject worldObject in worldObjects.Values)
				worldObject.Render(camera);

			// Render cameras
			foreach (Camera camera1 in worldCameras.Values)
				if (camera != camera1)
					camera1.Render(camera);
		}

		internalRenderer.SetDepthTest(false);
		foreach (Camera camera in uiCameras.Values)
		{
			camera.Update();
			internalRenderer.SetViewport(camera.ViewPort);

			// Render UI Objects
			foreach (RenderedObject uiObject in worldObjects.Values)
				uiObject.Render(camera);
		}

		internalRenderer.SwapBuffers();
	}

	private void WindowResized()
	{
		foreach (Camera camera in worldCameras.Values)
		{
			camera.Width = WindowSize.X;
			camera.Height = WindowSize.Y;
		}
		foreach (Camera camera in uiCameras.Values)
		{
			camera.Width = WindowSize.X;
			camera.Height = WindowSize.Y;
		}
	}

	#region Objects management
	public void UpdateObject(ref GameObjectData gameObjectData)
	{
		int id = gameObjectData.Id;
		IObject? gameObject = allObjects.ContainsKey(id) ? allObjects[id] : null;

		if (gameObject is not null)
		{
			if (gameObjectData.TransformDirty)
				gameObject.Transform.CopyFrom(gameObjectData.Transform);

			if (gameObjectData.MeshesDirty)
			{   // Update Meshes
				gameObject.Meshes.Clear();
				for (int i = 0; i < gameObjectData.Meshes.Count; i++)
					if (internalRenderer.MeshFactory.Create(gameObjectData.Meshes[i].Model, gameObjectData.Meshes[i].Material, out IMeshRenderer meshRenderer))
						gameObject.Meshes.Add(meshRenderer);
			}

			gameObject.Update();
		}
	}
	public void AddCamera(ref GameObjectData cameraData, ViewPort viewPort)
	{
		int id = cameraData.Id;
		IObject? gameObject = allObjects.ContainsKey(id) ? allObjects[id] : null;

		if (gameObject is null)
		{
			Camera camera = new Camera(cameraData.Id, cameraData.Transform.TranslateTransform(), (int)WindowSize.X, (int)WindowSize.Y, viewPort) { UI = cameraData.UI };
			if (cameraData.UI)
				uiCameras.Add(id, camera);
			else
				worldCameras.Add(id, camera);
			allObjects.Add(id, camera);

			camera.Update();
		}
		else
		{
			logger.LogError("Renderer. Camera id already exists, id: {id}", id);
		}
	}
	public void RemoveCamera(ref GameObjectData cameraData)
	{
		int id = cameraData.Id;
		IObject? gameObject = allObjects.ContainsKey(id) ? allObjects[id] : null;

		if (gameObject is not null)
		{
			if (cameraData.UI)
				uiCameras.Remove(id);
			else
				worldCameras.Remove(id);
			allObjects.Remove(id);
		}
		else
		{
			logger.LogError("Renderer. Camera id doesn't exist, id: {id}", id);
		}
	}
	public void AddGameObject(ref GameObjectData gameObjectData)
	{
		int id = gameObjectData.Id;
		IObject? gameObject = allObjects.ContainsKey(id) ? allObjects[id] : null;

		if (gameObject is null)
		{
			RenderedObject renderedObject = new RenderedObject(id, new Transform(gameObjectData.Transform));

			if (gameObjectData.TransformDirty)
				renderedObject.Transform.CopyFrom(gameObjectData.Transform);

			if (gameObjectData.MeshesDirty)
			{   // Update Meshes
				renderedObject.Meshes.Clear();
				for (int i = 0; i < gameObjectData.Meshes.Count; i++)
					if (internalRenderer.MeshFactory.Create(gameObjectData.Meshes[i].Model, gameObjectData.Meshes[i].Material, out IMeshRenderer meshRenderer))
						renderedObject.Meshes.Add(meshRenderer);
			}

			if (gameObjectData.UI)
				uiObjects.Add(id, renderedObject);
			else
				worldObjects.Add(id, renderedObject);
			allObjects.Add(id, renderedObject);

			renderedObject.Update();
		}
		else
		{
			logger.LogError("Renderer. Camera id already exists, id: {id}", id);
		}
	}
	public void RemoveGameObject(ref GameObjectData gameObjectData)
	{
		int id = gameObjectData.Id;
		IObject? gameObject = allObjects.ContainsKey(id) ? allObjects[id] : null;

		if (gameObject is not null)
		{
			if (gameObjectData.UI)
				uiObjects.Remove(id);
			else
				worldObjects.Remove(id);
			allObjects.Remove(id);
		}
		else
		{
			logger.LogError("Renderer. GameObject id doesn't exist");
		}
	}
	#endregion
}