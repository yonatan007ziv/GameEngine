using GameEngine.Core.API;
using GameEngine.Core.Components;
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
	private RenderedCamera camera;

	private readonly List<RenderedObject> renderedObjects = new List<RenderedObject>();

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
	}

	public override void Render()
	{
		for (int i = 0; i < renderedObjects.Count; i++)
			renderedObjects[i].Render(camera);
	}

	public void SetCamera(ref GameObjectData gameObjectData)
	{
		if (camera != null)
			renderedObjects.Remove(camera);

		camera = new RenderedCamera(gameObjectData.Id, gameObjectData.Transform, Width, Height);
		renderedObjects.Add(camera);
	}

	public void UpdateGameObject(ref GameObjectData gameObjectData)
	{
		int updateId = gameObjectData.Id;
		RenderedObject? gameObject = renderedObjects.Find(obj => obj.Id == updateId);

		if (gameObject is null)
		{
			IMeshRenderer[] meshRenderers = new IMeshRenderer[gameObjectData.Meshes.Count];
			for (int i = 0; i < gameObjectData.Meshes.Count; i++)
			{
				meshFactory.Create(gameObjectData.Meshes[i].Model, gameObjectData.Meshes[i].Material, out IMeshRenderer renderer);
				meshRenderers[i] = renderer;
			}

			gameObject = new RenderedObject(gameObjectData.Transform, gameObjectData.Id, meshRenderers);
			renderedObjects.Add(gameObject);
		}
		else if (gameObjectData.MeshesDirty)
		{   // Update Meshes
			gameObject.Meshes.Clear();
			for (int i = 0; i < gameObjectData.Meshes.Count; i++)
				if (meshFactory.Create(gameObjectData.Meshes[i].Model, gameObjectData.Meshes[i].Material, out IMeshRenderer meshRenderer))
					gameObject.Meshes.Add(meshRenderer);
		}

		gameObject.Update();
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