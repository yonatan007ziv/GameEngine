using Microsoft.Extensions.Logging;
using OpenGLRenderer.Components;
using OpenGLRenderer.Models;
using OpenGLRenderer.OpenGL;
using OpenGLRenderer.OpenGL.Meshes;
using OpenGLRenderer.Resources.Shaders.Managed;
using OpenGLRenderer.Services.Interfaces.OpenGL;
using OpenGLRenderer.Services.Interfaces.Utils.Managers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenGLRenderer.Services.Implementations.OpenGL.Renderer;

internal class Renderer : BaseRenderer
{
	private readonly ILogger logger;
	private readonly IGameObjectManager gameObjectManager;
	private readonly IShaderManager shaderManager;
	private readonly IModelImporter importer;
	private readonly ISceneManager sceneManager;

	// Temps
	private Camera camera;
	private List<GameObject> sceneObjects = new List<GameObject>();
	private ShaderProgram shader;

	public Renderer(ILogger logger, IGameObjectManager gameObjectManager, IShaderManager shaderManager, ISettingsManager settingsManager, IModelImporter importer, ISceneManager sceneManager)
		: base(settingsManager)
	{
		this.logger = logger;
		this.gameObjectManager = gameObjectManager;
		this.shaderManager = shaderManager;
		this.importer = importer;
		this.sceneManager = sceneManager;

		CenterWindow(new Vector2i(width, height));
	}

	protected override void RenderFrame(float deltaTime)
	{
		sceneManager.CurrentScene.RenderScene(deltaTime);

		shaderManager.BindShader(shader);

		foreach (GameObject gameObject in sceneObjects)
			gameObject.Render();
	}

	protected override void UpdateFrame(float deltaTime)
	{
		sceneManager.CurrentScene.UpdateScene(deltaTime);

		// logger.LogDebug("Fps: {fps}", 1 / deltaTime);

		int shaderId = shaderManager.ActiveShader.Id;
		Matrix4 view = camera.ViewMatrix;
		int viewLoc = GL.GetUniformLocation(shaderId, "view");
		GL.UniformMatrix4(viewLoc, true, ref view);

		Matrix4 projection = camera.ProjectionMatrix;
		int projectionLoc = GL.GetUniformLocation(shaderId, "projection");
		GL.UniformMatrix4(projectionLoc, true, ref projection);

		camera.Update(MouseState, KeyboardState, deltaTime);
	}

	protected override void Load()
	{
		logger.LogInformation("Loading Renderer...");
		sceneManager.LoadScene("MainScene.scene");

		shader = new DefaultShader();
		shaderManager.RegisterShader(shader);
		shaderManager.BindShader(shader);

		sceneObjects.Add(gameObjectManager.CreateGameObject());
		sceneObjects.Add(gameObjectManager.CreateGameObject());
		sceneObjects.Add(gameObjectManager.CreateGameObject());

		//ModelData pyramidData = importer.ImportModel("Pyramid.obj");
		//ModelData humanData = importer.ImportModel("Human1.obj");
		ModelData dragonData = importer.ImportModel("smaug.obj");
		ModelData bugattiData = importer.ImportModel("Bugatti1.obj");
		//ModelData treeData = importer.ImportModel("Tree1.obj");

		camera = new Camera(sceneObjects[0], 10, 10, width, height);

		sceneObjects[0].Transform.Position = new Vector3(0, 5, 10);

		//sceneObjects[0].Mesh = new CustomMesh(pyramidData);
		//sceneObjects[0].Mesh = new CustomMesh(humanData);
		sceneObjects[1].Mesh = new CustomMesh(dragonData, shaderManager);
		sceneObjects[2].Mesh = new CustomMesh(bugattiData, shaderManager);
		// sceneObjects[0].Mesh = new CustomMesh(treeData);
	}

	protected override void Unload()
	{
		shaderManager.DisposeAll();
	}

	protected override void GLDebugCallback(string msg, DebugSeverity severity)
	{
		switch (severity)
		{
			case DebugSeverity.DebugSeverityLow:
				logger.LogInformation("GL-INFO: {errorCode}", msg);
				break;
			case DebugSeverity.DebugSeverityMedium:
				logger.LogError("GL-ERROR: {errorCode}", msg);
				break;
			case DebugSeverity.DebugSeverityHigh:
				logger.LogCritical("GL-CRITICAL: {errorCode}", msg);
				break;
			case DebugSeverity.DebugSeverityNotification:
				logger.LogTrace("GL-TRACE: {errorCode}", msg);
				break;
		}
	}
}