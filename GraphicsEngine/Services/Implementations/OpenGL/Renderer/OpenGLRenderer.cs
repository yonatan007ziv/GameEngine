using GameEngine.Core.API;
using GameEngine.Core.Components;
using GameEngine.Core.SharedServices.Interfaces;
using GraphicsEngine.Components.Interfaces;
using GraphicsEngine.Components.OpenGL;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Components.Shared.Data;
using Microsoft.Extensions.Logging;
using OpenTK.Graphics.OpenGL4;
using System;

namespace GraphicsEngine.Services.Implementations.OpenGL.Renderer;

public class OpenGLRenderer : BaseOpenGLRenderer, IGraphicsEngine
{
	public static OpenGLRenderer Instance; // TEMP

	private readonly ILogger logger;
	private readonly RendererCamera camera;

	public List<GameObject> RenderingObjects { get; } = new List<GameObject>();
	public Transform CameraTransform => camera.Transform;

	IMeshRenderer mesh;

	public new string Title
	{
		get => base.Title;
		set => base.Title = value;
	}

	public OpenGLRenderer(ILogger logger, IFactory<string, ModelData> modelFactory, IFactory<string, Material> materialFactory)
	{
		Instance = this;
		camera = new RendererCamera(new Transform(), Width, Height);

		modelFactory.Create("TREX.obj", out ModelData modelData);
		materialFactory.Create("Textured", out Material material);
		mesh = new OpenGLMeshRenderer(modelData, material);
		this.logger = logger;
		CameraTransform.Position -= new System.Numerics.Vector3(0, 0, 10);
	}

	public void SyncState()
	{

	}

	public override void Render()
	{
		camera.Update();
		mesh.Render(camera);
		//foreach (GameObject gameObject in RenderingObjects)
		//	gameObject.Render(camera);
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