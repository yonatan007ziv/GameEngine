using GameEngine.Core.API;
using GameEngine.Core.Components;
using GameEngine.Core.Components.CommunicationComponentsData;
using GameEngine.Core.SharedServices.Interfaces;
using GraphicsEngine.Components.Interfaces;
using GraphicsEngine.Components.Shared;
using Microsoft.Extensions.Logging;
using OpenTK.Graphics.OpenGL4;

namespace GraphicsEngine.Services.Implementations.OpenGL.Renderer;

internal class OpenGLRenderer : BaseOpenGLRenderer, IGraphicsEngine
{
	public static OpenGLRenderer Instance; // TEMP

	private readonly ILogger logger;
	private readonly IFactory<string, string, IMeshRenderer> meshFactory;
	private readonly RenderedCamera camera;

	private readonly List<RenderedGameObject> gameObjects = new List<RenderedGameObject>();
	public Transform CameraTransform => camera.Transform;

	public new string Title
	{
		get => base.Title;
		set => base.Title = value;
	}

	public OpenGLRenderer(ILogger logger, IFactory<string, string, IMeshRenderer> meshFactory)
	{
		this.logger = logger;
		this.meshFactory = meshFactory;

		Instance = this;
		camera = new RenderedCamera(new Transform(), Width, Height);
	}

	public override void Render()
	{
		foreach (RenderedGameObject gameObject in gameObjects)
			gameObject.Render(camera);
	}

	public void Update()
	{
		foreach (RenderedGameObject gameObject in gameObjects)
			gameObject.Update();

		camera.Update();
	}

	public void SetCameraParent(ref GameObjectData gameObjectData)
	{
		foreach (var gameObject in gameObjects)
			if (gameObject.Id == gameObjectData.Id)
				camera.Transform = gameObject.Transform;
	}

	public void RegisterGameObject(ref GameObjectData gameObjectData)
	{
		IMeshRenderer[] meshRenderers = new IMeshRenderer[gameObjectData.Meshes.Count];
		for (int i = 0; i < gameObjectData.Meshes.Count; i++)
		{
			meshFactory.Create(gameObjectData.Meshes[i].Model, gameObjectData.Meshes[i].Material, out IMeshRenderer renderer);
			meshRenderers[i] = renderer;
		}

		gameObjects.Add(new RenderedGameObject(new Transform(gameObjectData.Transform), gameObjectData.Id, meshRenderers));
	}

	public void UpdateGameObject(ref GameObjectData gameObjectData)
	{
		logger.LogInformation("Updating GameObjectId: {id}...", gameObjectData.Id);
		camera.Update();

		int updateId = gameObjectData.Id;
		RenderedGameObject? gameObject = gameObjects.Find(obj => obj.Id == updateId);

		if (gameObject is null)
			logger.LogInformation("Can't find gameobject to update, Id: {id}", gameObjectData.Id);
		else
		{
			// Update Transform
			if (gameObjectData.TransformDirty)
				gameObject.Transform.Copy(gameObjectData.Transform);

			// Update Meshes
			if (gameObjectData.MeshesDirty)
			{
				gameObject.Meshes.Clear();
				foreach (MeshData meshData in gameObjectData.Meshes)
					if (meshFactory.Create(meshData.Model, meshData.Material, out IMeshRenderer meshRenderer))
						gameObject.Meshes.Add(meshRenderer);
			}
		}
		// read message queue
	}

	protected override void Load() { }

	protected override void Unload() { }

	protected override void Resize(int width, int height)
	{
		camera.Width = width;
		camera.Height = height;
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