using GameEngine.Components.GameObjectComponents;
using GameEngine.Core.API;
using GameEngine.Core.Components.CommunicationComponentsData;
using GameEngine.Services.Interfaces.Managers;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace GameEngine.Services.Implementations;

internal class GameEngine : IGameEngine
{
	private readonly ILogger logger;
	private readonly IGraphicsEngine renderer;
	private readonly IPhysicsEngine physicsEngine;
	private readonly IGameObjectManager gameObjectManager;

	private GameObject camera;

	private readonly Stopwatch engineTime;
	public float ElapsedMs => engineTime.ElapsedMilliseconds;

	public GameEngine(ILogger logger, IGraphicsEngine renderer, IPhysicsEngine physicsEngine, IGameObjectManager gameObjectManager)
	{
		this.logger = logger;
		this.renderer = renderer;
		this.physicsEngine = physicsEngine;
		this.gameObjectManager = gameObjectManager;

		engineTime = new Stopwatch();
		engineTime.Start();

		camera = gameObjectManager.CreateGameObject();

		Start();
	}

	private void Start()
	{
		renderer.Start();

		GameObjectData cameraData = camera.TranslateGameObject();
		renderer.RegisterGameObject(ref cameraData);
		renderer.SetCameraParent(ref cameraData);

		//EngineGameObject trex = gameObjectManager.CreateGameObject();
		//EngineGameObject leed = gameObjectManager.CreateGameObject();
		//
		//trex.Meshes.Add(new Core.Components.MeshData("Trex.obj", new Core.Components.MaterialData("Textured", "TrexTexture.png")));
		//SharedGameObject trexData = trex.TranslateGameObject();
		//renderer.RegisterGameObject(ref trexData);
		//
		//leed.Meshes.Add(new Core.Components.MeshData("Leed.obj", new Core.Components.MaterialData("Textured", "Red.png")));
		//SharedGameObject leedData = leed.TranslateGameObject();
		//renderer.RegisterGameObject(ref leedData);
		//
		//trex.Transform.Scale = new System.Numerics.Vector3(1, 1, 1);
		//leed.Transform.Scale = new System.Numerics.Vector3(1, 1, 1);

		camera.Transform.Rotation = new System.Numerics.Vector3(-90, 120, 0);

		for (int i = 0; i < MathF.PI * 25 * 4; i++)
		{
			GameObject gameObject = gameObjectManager.CreateGameObject();
			gameObject.Transform.Scale = new System.Numerics.Vector3(1, 1, 1);
			gameObject.Transform.Position = new System.Numerics.Vector3(MathF.Sin(i / 25f) * 50, 0, MathF.Cos(i / 25f) * 50);
			gameObject.Transform.Rotation = new System.Numerics.Vector3(MathF.Sin(i / 25f) * 50, 0, MathF.Cos(i / 25f) * 50);
			gameObject.Meshes.Add(new Core.Components.MeshData("Trex.obj", i % 2 == 0 ? "MissingTexture.mat" : "Trex.mat"));
			GameObjectData sharedGameObject = gameObject.TranslateGameObject();
			renderer.RegisterGameObject(ref sharedGameObject);
		}
	}

	public void Run()
	{
		renderer.TurnVSync(true);
		renderer.LockMouse(false);

		while (true)
		{
			SyncEngines();

			// bounce bounce 
			//camera.Transform.Position = new System.Numerics.Vector3(-150, MathF.Sin((float)engineTime.Elapsed.TotalSeconds * 15) / 4, -100);
			camera.Transform.Position += new System.Numerics.Vector3(0, 0.05f, 0);

			renderer.RenderFrame();
		}
	}

	private void SyncEngines()
	{
		foreach (GameObject gameObject in gameObjectManager.GameObjects)
		{
			if (!gameObject.IsDirty())
				continue;

			GameObjectData comGameObject = gameObject.TranslateGameObject();

			// Renderer
			// probably use IGameObjectManager to track changes and or use a message queue for GameObject id
			renderer.UpdateGameObject(ref comGameObject);

			gameObject.ResetDirty();
		}
		renderer.Update();
	}
}