using GameEngine.Core.API;
using GameEngine.Core.Components;
using GameEngine.Core.Extensions;
using GameEngine.Core.SharedServices.Interfaces;
using GraphicsEngine.Components.Interfaces;
using GraphicsEngine.Components.Shared;
using Microsoft.Extensions.Logging;
using OpenTK.Graphics.OpenGL4;

namespace GraphicsEngine.Services.Implementations.OpenGL.Renderer;

internal class OpenGLRenderer : BaseOpenGLRenderer, IGraphicsEngine
{
	private readonly ILogger logger;
	private readonly IFactory<string, string, IMeshRenderer> meshFactory;

	private readonly List<Camera> cameras = new List<Camera>();
	private readonly List<Camera> uiCameras = new List<Camera>();

	private readonly List<IObject> updateableObjects = new List<IObject>();
	private readonly List<RenderedObject> renderedObjects = new List<RenderedObject>();
	private readonly List<RenderedObject> uiObjects = new List<RenderedObject>();

	public new string Title
	{
		get => base.Title;
		set => base.Title = value;
	}

	public OpenGLRenderer(ILogger logger, IFactory<string, string, IMeshRenderer> meshFactory)
	{
		this.logger = logger;
		this.meshFactory = meshFactory;
	}

	public override void Render()
	{
		GL.Enable(EnableCap.DepthTest);

		foreach (Camera camera in cameras)
		{
			camera.Update();
			GL.Viewport((int)((camera.ViewPort.x - camera.ViewPort.width / 2) * Width), (int)((camera.ViewPort.y - camera.ViewPort.height / 2) * Height), (int)(camera.ViewPort.width * Width), (int)(camera.ViewPort.height * Height));
			for (int i = 0; i < renderedObjects.Count; i++)
				renderedObjects[i].Render(camera);
			foreach (Camera camera1 in cameras)
				if (camera != camera1)
					camera1.Render(camera);
		}

		GL.Disable(EnableCap.DepthTest);
		foreach (Camera camera in uiCameras)
		{
			camera.Update();
			GL.Viewport((int)((camera.ViewPort.x - camera.ViewPort.width / 2) * Width), (int)((camera.ViewPort.y - camera.ViewPort.height / 2) * Height), (int)(camera.ViewPort.width * Width), (int)(camera.ViewPort.height * Height));
			for (int i = 0; i < uiObjects.Count; i++)
				uiObjects[i].Render(camera);
		}
	}

	public void RegisterCameraGameObject(ref GameObjectData cameraData, ViewPort viewPort)
	{
		Camera camera;
		if (cameraData.UI)
			uiCameras.Add(camera = new Camera(cameraData.Id, cameraData.Transform.TranslateTransform(), Width, Height, viewPort) { UI = true });
		else
			cameras.Add(camera = new Camera(cameraData.Id, cameraData.Transform.TranslateTransform(), Width, Height, viewPort) { UI = false });
		updateableObjects.Add(camera);
	}

	public void RegisterObject(ref GameObjectData gameObjectData)
	{
		int updateId = gameObjectData.Id;
		IObject? gameObject = updateableObjects.Find(obj => obj.Id == updateId);

		if (gameObject is null)
		{
			RenderedObject renderedObject = new RenderedObject(gameObjectData.Id, new Transform().CopyFrom(gameObjectData.Transform));

			if (gameObjectData.UI)
				uiObjects.Add(renderedObject);
			else
				renderedObjects.Add(renderedObject);
			updateableObjects.Add(renderedObject);
		}
	}

	public void UpdateObject(ref GameObjectData gameObjectData)
	{
		int updateId = gameObjectData.Id;
		IObject? gameObject = updateableObjects.Find(obj => obj.Id == updateId);

		if (gameObject is not null)
		{
			if (gameObjectData.TransformDirty)
				gameObject.Transform.CopyFrom(gameObjectData.Transform);

			if (gameObjectData.MeshesDirty)
			{   // Update Meshes
				gameObject.Meshes.Clear();
				for (int i = 0; i < gameObjectData.Meshes.Count; i++)
					if (meshFactory.Create(gameObjectData.Meshes[i].Model, gameObjectData.Meshes[i].Material, out IMeshRenderer meshRenderer))
						gameObject.Meshes.Add(meshRenderer);
			}

			gameObject.Update();
		}
	}

	protected override void Load() { }

	protected override void Unload() { }

	protected override void Resize(int width, int height)
	{
		foreach (Camera camera in cameras)
		{
			camera.Width = width;
			camera.Height = height;
		}
	}

	protected override void GLDebugCallback(string msg, DebugSeverity severity)
	{
		switch (severity)
		{
			case DebugSeverity.DebugSeverityLow:
				logger.LogInformation("GL-INFO: {errorMsg}", msg);
				break;
			case DebugSeverity.DebugSeverityMedium:
				logger.LogError("GL-ERROR: {errorMsg}", msg);
				break;
			case DebugSeverity.DebugSeverityHigh:
				logger.LogCritical("GL-CRITICAL: {errorMsg}", msg);
				break;
			case DebugSeverity.DebugSeverityNotification:
				logger.LogTrace("GL-TRACE: {errorMsg}", msg);
				break;
		}
	}
}