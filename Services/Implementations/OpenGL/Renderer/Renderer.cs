using Microsoft.Extensions.Logging;
using OpenGLRenderer.Components;
using OpenGLRenderer.Models;
using OpenGLRenderer.OpenGL;
using OpenGLRenderer.OpenGL.Meshes;
using OpenGLRenderer.Services.Interfaces.OpenGL;
using OpenGLRenderer.Services.Interfaces.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenGLRenderer.Services.Implementations.OpenGL.Renderer;

internal class Renderer : BaseRenderer
{
	private readonly ILogger logger;
	private readonly IModelImporter importer;
	private Scene currentScene;


	// Temps
	private float yAngle;
	private Camera camera;
	private List<GameObject> sceneObjects = new List<GameObject>();
	private ShaderProgram shader;

	public Renderer(ILogger logger, ISettingsManager settingsManager, IModelImporter importer, IFactory<Scene> sceneFactory)
		: base(settingsManager)
	{
		this.logger = logger;
		this.importer = importer;

		CenterWindow(new Vector2i(width, height));

		// Does not deal with OpenGL logic yet due to OpenGL not binding at this time.
		currentScene = sceneFactory.Create();
	}

	protected override void RenderFrame(float deltaTime)
	{
		foreach (GameObject gameObject in sceneObjects)
			gameObject.Draw();
	}

	protected override void UpdateFrame(float deltaTime)
	{
		// logger.LogDebug("Fps: {0}", 1000 / deltaTime);

		float sin = (float)Math.Sin(2.5 * GLFW.GetTime()) / 2 + .5f;
		float cos = (float)Math.Cos(2.5 * GLFW.GetTime()) / 2 + .5f;
		int cL = GL.GetUniformLocation(shader.Id, "Color");
		GL.Uniform3(cL, sin, cos, sin);

		Matrix4 model = Matrix4.CreateRotationY(yAngle);
		// yAngle += deltaTime;
		int modelLoc = GL.GetUniformLocation(shader.Id, "model");
		GL.UniformMatrix4(modelLoc, true, ref model);

		Matrix4 view = camera.ViewMatrix;
		int viewLoc = GL.GetUniformLocation(shader.Id, "view");
		GL.UniformMatrix4(viewLoc, true, ref view);

		Matrix4 projection = camera.ProjectionMatrix;
		int projectionLoc = GL.GetUniformLocation(shader.Id, "projection");
		GL.UniformMatrix4(projectionLoc, true, ref projection);

		camera.Update(MouseState, KeyboardState, deltaTime);
	}

	protected override void Load()
	{
		logger.LogInformation("Loading Renderer...");

		// currentScene.Load("MainScene.scene");

		shader = new ShaderProgram(new ShaderSource("DefVertex.glsl"), new ShaderSource("DefFragment.glsl"));

		sceneObjects.Add(new GameObject(Vector3.Zero));
		sceneObjects.Add(new GameObject(Vector3.Zero));

		ModelData pyramidData = importer.ImportModel(@"D:\Code\VS Community\OpenGLRenderer\Resources\Pyramid.obj");
		ModelData humanData = importer.ImportModel(@"D:\Code\VS Community\OpenGLRenderer\Resources\Human1.obj");
		ModelData dragonData = importer.ImportModel(@"D:\Code\VS Community\OpenGLRenderer\Resources\smaug.obj");
		ModelData bugattiData = importer.ImportModel(@"D:\Code\VS Community\OpenGLRenderer\Resources\Bugatti1.obj");
		ModelData treeData = importer.ImportModel(@"D:\Code\VS Community\OpenGLRenderer\Resources\Tree1.obj");

		camera = new Camera(sceneObjects[0], 10, 10, width, height);

		//_ = new CustomMesh(sceneObjects[1], pyramidData, shader);
		_ = new CustomMesh(sceneObjects[1], humanData, shader);
		_ = new CustomMesh(sceneObjects[1], dragonData, shader);
		//_ = new CustomMesh(sceneObjects[1], bugattiData, shader);
		//_ = new CustomMesh(sceneObjects[1], treeData, shader);
	}

	protected override void Unload()
	{
		shader.Delete();
		foreach (GameObject gameObject in sceneObjects)
			gameObject.Delete();
	}
}