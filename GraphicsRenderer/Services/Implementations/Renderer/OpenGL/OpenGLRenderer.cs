using GraphicsRenderer.Components.Extensions;
using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Components.OpenGL;
using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Components.Shared.Data;
using GraphicsRenderer.Services.Interfaces.Renderer;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;
using Microsoft.Extensions.Logging;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace GraphicsRenderer.Services.Implementations.Renderer.OpenGL;

internal class OpenGLRenderer : BaseOpenGLRenderer, IRenderer
{
	private readonly ILogger logger;
	private readonly IGameObjectManager gameObjectManager;
	private readonly IShaderManager shaderManager;
	private readonly IModelImporter importer;
	private readonly ISceneManager sceneManager;

	// Temps
	public ICamera camera;

	public OpenGLRenderer(ILogger logger, IGameObjectManager gameObjectManager, IShaderManager shaderManager, ISettingsManager settingsManager, IModelImporter importer, ISceneManager sceneManager)
		: base(settingsManager)
	{
		this.logger = logger;
		this.gameObjectManager = gameObjectManager;
		this.shaderManager = shaderManager;
		this.importer = importer;
		this.sceneManager = sceneManager;

		//CenterWindow(new Vector2i(Width, Height));
	}

	protected override void RenderFrame(float deltaTime)
	{
		sceneManager.CurrentScene.RenderScene(deltaTime);

		foreach (GameObject gameObject in gameObjectManager.GameObjects)
			gameObject.Render(camera);
	}

	protected override void UpdateFrame(float deltaTime)
	{
		sceneManager.CurrentScene.UpdateScene(deltaTime);

		camera.Update(MouseState.Position.ToNumerics(), deltaTime);
	}

	protected override void Load()
	{
		shaderManager.RegisterShaders();

		CursorState = CursorState.Grabbed;

		logger.LogInformation("Loading Renderer...");
		sceneManager.LoadScene("MainScene.scene");

		gameObjectManager.CreateGameObject();
		gameObjectManager.CreateGameObject();

		gameObjectManager.GameObjects[0].Transform.Position = new Vector3(0, 0, 40);
		camera = new OpenGLCamera(gameObjectManager.GameObjects[0], 50, 10, Width, Height);

		Model3DData trexData = importer.ImportModel("TREX.obj");

		var defaultShader = shaderManager.GetShader("Default");
		var texturedShader = shaderManager.GetShader("Textured");

		gameObjectManager.GameObjects[1].Mesh = new OpenGLMesh(trexData, shaderManager);
		gameObjectManager.GameObjects[1].shader = texturedShader;
		// gameObjectManager.GameObjects[1].Gizmos = new GizmosBoxMesh(trexData.BoundingBox, shaderManager);
	}

	protected override void Unload()
	{
		shaderManager.DisposeAll();
	}

	protected override void Resize(int width, int height)
	{
		if (camera != null)
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