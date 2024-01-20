using GameEngine.Components.GameObjectComponents;
using GameEngine.Core.API;
using GameEngine.Core.Components;
using GameEngine.Core.Components.CommunicationComponentsData;
using GameEngine.Services.Interfaces.Managers;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace GameEngine.Services.Implementations;

internal class GameEngine : IGameEngine
{
	private readonly ILogger logger;
	private readonly IGraphicsEngine renderer;
	private readonly ISoundEngine soundEngine;
	private readonly IInputEngine inputEngine;
	private readonly IPhysicsEngine physicsEngine;
	private readonly IGameObjectManager gameObjectManager;

	private GameObject camera;

	private readonly Stopwatch fpsTime, tickTime, engineTime;

	public int TickRate { get; set; } = 128;
	public int FpsCap { get; set; } = 240;

	public IntPtr WindowHandle => renderer.WindowHandle;
	public float DeltaTime { get => (float)fpsTime.Elapsed.TotalSeconds; }
	public float ElapsedSeconds => (float)engineTime.Elapsed.TotalSeconds;

	public GameEngine(ILogger logger, IGraphicsEngine renderer, ISoundEngine soundEngine, IInputEngine inputEngine, IPhysicsEngine physicsEngine, IGameObjectManager gameObjectManager)
	{
		this.logger = logger;
		this.renderer = renderer;
		this.soundEngine = soundEngine;
		this.inputEngine = inputEngine;
		this.physicsEngine = physicsEngine;
		this.gameObjectManager = gameObjectManager;

		fpsTime = new Stopwatch();
		tickTime = new Stopwatch();
		engineTime = new Stopwatch();
		engineTime.Start();

		camera = gameObjectManager.CreateGameObject();
	}

	public void Run()
	{
		camera.Transform.Position = new System.Numerics.Vector3(0, 75, 0);
		camera.Transform.Rotation = new System.Numerics.Vector3(-90, 0, 0);
		GameObjectData cameraObj = camera.TranslateGameObject();
		renderer.SetCamera(ref cameraObj);
		physicsEngine.UpdateGameObject(ref cameraObj);

		GameObject gameObject = gameObjectManager.CreateGameObject();
		gameObject.Transform.Scale = new System.Numerics.Vector3(0.5f, 1, 1);
		gameObject.Meshes.Add(new MeshData("Trex.obj", "Trex.mat"));


		// Run on a different thread: object ownership issues
		// UpdateLoop();
		RenderLoop();
	}

	//private async void UpdateLoop()
	//{
	//	// Thread.CurrentThread.Name = "Update Thread";

	//	while (true)
	//	{
	//		tickTime.Restart();

	//		physicsEngine.PhysicsPass(DeltaTime);
	//		SyncEngines();

	//		// Tick Rate
	//		int timeToWait = 1000 / TickRate - (int)tickTime.ElapsedMilliseconds;
	//		await Task.Delay(timeToWait > 0 ? timeToWait : 0);
	//	}
	//}

	private void RenderLoop()
	{
		soundEngine.AttachWnd(WindowHandle);
		soundEngine.Test();

		Thread.CurrentThread.Name = "Render Thread";

		renderer.Start();

		while (true)
		{
			fpsTime.Restart();

			renderer.RenderFrame();

			// Fps Cap
			int timeToWait = 1000 / FpsCap - (int)fpsTime.ElapsedMilliseconds;
			Thread.Sleep(timeToWait > 0 ? timeToWait : 0);

			{   // temp: Move to another thread
				physicsEngine.PhysicsPass(DeltaTime);
				SyncEngines();
			}

			if (DeltaTime != 0)
				logger.LogInformation("DT {dt}", DeltaTime);
			logger.LogInformation("POS {dt}", camera.Transform.Position);
		}
	}

	private bool first = true;
	private void SyncEngines()
	{
		if (first)
		{
			GameObjectData cameraData = camera.TranslateGameObject();
			renderer.SetCamera(ref cameraData);
			first = false;
		}

		for (int i = 0; i < gameObjectManager.GameObjects.Count; i++)
		{
			GameObject gameObject = gameObjectManager.GameObjects[i];
			if (!gameObject.Dirty)
				continue;

			GameObjectData comGameObject = gameObject.TranslateGameObject();

			// Graphics Engine
			renderer.UpdateGameObject(ref comGameObject);

			// Physics Engine
			physicsEngine.UpdateGameObject(ref comGameObject);

			// Sound Engine
			// TODO: update sound objects

			gameObject.Dirty = false;
		}
	}
}