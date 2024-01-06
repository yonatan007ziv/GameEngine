using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Services.Interfaces.InputProviders;
using GraphicsRenderer.Services.Interfaces.Renderer;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;
using Microsoft.Extensions.Logging;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;

namespace GraphicsRenderer.Services.Implementations.OpenGL.Renderer;

public class OpenGLRenderer : BaseOpenGLRenderer, IRenderer
{
	public static OpenGLRenderer Instance; // TEMP

	private readonly ILogger logger;
	private readonly IGameObjectManager gameObjectManager;
	private readonly ICamera camera;

	public OpenGLRenderer(ILogger logger, IGameObjectManager gameObjectManager, IInputProvider inputProvider, IFactory<string, string, string, GameObject> materializedGameObjectFactory)
	{
		Instance = this;

		this.logger = logger;
		this.gameObjectManager = gameObjectManager;

		LockMouse(true);
		TurnVSync(true);

		GameObject player = gameObjectManager.CreateGameObject();
		player.Transform.Position = new Vector3(0, 0, 0);
		player.Components.Add(camera = new FreeCameraController(inputProvider, player, 10, Width, Height));

		materializedGameObjectFactory.Create("leed.obj", "Textured", "BackgroundTest.png", out GameObject leed);
		leed.Transform.Position += new Vector3(-50, 0, 0);

		materializedGameObjectFactory.Create("Human1.obj", "Textured", "BackgroundTest.png", out GameObject human);
		human.Transform.Position += new Vector3(0, 0, 0);

		materializedGameObjectFactory.Create("TREX.obj", "Textured", "TrexTexture.png", out GameObject trex);
		trex.Transform.Position += new Vector3(50, 0, 0);
	}

	protected override void RenderFrame()
	{
		foreach (GameObject gameObject in gameObjectManager.GameObjects)
			gameObject.Render(camera);
	}

	protected override void UpdateFrame(float deltaTime)
	{
		foreach (GameObject gameObject in gameObjectManager.GameObjects)
			gameObject.Update(deltaTime);
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