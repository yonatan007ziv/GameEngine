using Microsoft.Extensions.Logging;
using OpenGLRenderer.Components;
using OpenGLRenderer.Models;
using OpenGLRenderer.OpenGL;
using OpenGLRenderer.OpenGL.Meshes;
using OpenGLRenderer.Services.Interfaces.OpenGL;
using OpenGLRenderer.Services.Interfaces.Utils;
using OpenGLRenderer.Services.Interfaces.Utils.Factories;
using OpenGLRenderer.Services.Interfaces.Utils.Managers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenGLRenderer.Services.Implementations.OpenGL.Renderer;

internal class Renderer : BaseRenderer
{
	private readonly ILogger logger;
	private readonly IFactory<string, string, ShaderProgram> shaderProgramFactory;
	private readonly IGameObjectManager gameObjectManager;
	private readonly IShaderManager shaderManager;
	private readonly IShaderBank shaderBank;
	private readonly IModelImporter importer;
	private readonly ISceneManager sceneManager;

	// Temps
	public Camera camera;
	private List<GameObject> sceneObjects = new List<GameObject>();

	public Renderer(ILogger logger, IFactory<string, string, ShaderProgram> shaderProgramFactory, IGameObjectManager gameObjectManager, IShaderManager shaderManager, IShaderBank shaderBank, ISettingsManager settingsManager, IModelImporter importer, ISceneManager sceneManager)
		: base(settingsManager)
	{
		this.logger = logger;
		this.shaderProgramFactory = shaderProgramFactory;
		this.gameObjectManager = gameObjectManager;
		this.shaderManager = shaderManager;
		this.shaderBank = shaderBank;
		this.importer = importer;
		this.sceneManager = sceneManager;

		CenterWindow(new Vector2i(width, height));
	}

	protected override void RenderFrame(float deltaTime)
	{
		sceneManager.CurrentScene.RenderScene(deltaTime);

		foreach (GameObject gameObject in sceneObjects)
			gameObject.Render(camera);
	}

	private float frameSum = 0; int frameCounter = 0;
	protected override void UpdateFrame(float deltaTime)
	{
		sceneManager.CurrentScene.UpdateScene(deltaTime);

		sceneObjects[1].Transform.Rotation -= new Vector3(0, 0, deltaTime);
		sceneObjects[2].Transform.Rotation += new Vector3(0, 0, deltaTime);
		sceneObjects[3].Transform.Rotation += new Vector3(0, deltaTime, 0);

		frameSum += 1 / deltaTime;
		logger.LogDebug("Fps: {fps} : Avg Fps: {fps}", 1 / deltaTime, frameSum / frameCounter++);

		camera.Update(MouseState, KeyboardState, deltaTime);
	}

	protected override void Load()
	{
		logger.LogInformation("Loading Renderer...");
		sceneManager.LoadScene("MainScene.scene");

		shaderBank.RegisterShaders(shaderManager);

		sceneObjects.Add(gameObjectManager.CreateGameObject());
		sceneObjects.Add(gameObjectManager.CreateGameObject());
		sceneObjects.Add(gameObjectManager.CreateGameObject());
		sceneObjects.Add(gameObjectManager.CreateGameObject());

		sceneObjects[0].Transform.Position = new Vector3(0, 5, 10);

		ModelData pyramidData = importer.ImportModel("Pyramid.obj");
		ModelData humanData = importer.ImportModel("Human1.obj");
		ModelData dragonData = importer.ImportModel("smaug.obj");
		//ModelData bugattiData = importer.ImportModel("Bugatti1.obj");
		//ModelData treeData = importer.ImportModel("Tree1.obj");

		camera = new Camera(sceneObjects[0], 10, 10, width, height);

		for (int i = 0; i < 0; i++)
			for (int j = 0; j < 0; j++)
			{
				GameObject gameObject = gameObjectManager.CreateGameObject();
				gameObject.Mesh = new CustomMesh(humanData, shaderManager);
				gameObject.Box = new GizmosBoxMesh(humanData.BoundingBox, shaderManager);
				gameObject.Transform.Position = new Vector3(10 * i, 0, -10 * j);

				sceneObjects.Add(gameObject);
			}

		sceneObjects[1].Mesh = new CustomMesh(pyramidData, shaderManager);
		sceneObjects[1].Box = new GizmosBoxMesh(pyramidData.BoundingBox, shaderManager);
		sceneObjects[2].Mesh = new CustomMesh(humanData, shaderManager);
		sceneObjects[2].Box = new GizmosBoxMesh(humanData.BoundingBox, shaderManager);
		sceneObjects[3].Mesh = new CustomMesh(dragonData, shaderManager);
		sceneObjects[3].Box = new GizmosBoxMesh(dragonData.BoundingBox, shaderManager);
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