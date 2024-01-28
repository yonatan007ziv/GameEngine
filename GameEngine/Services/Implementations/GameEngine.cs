using GameEngine.Components.GameObjectComponents;
using GameEngine.Core.API;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Physics;
using GameEngine.Extensions;
using GameEngine.Services.Interfaces.Managers;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Numerics;

namespace GameEngine.Services.Implementations;

internal class GameEngine : IGameEngine
{
	private readonly ILogger logger;
	private readonly IGraphicsEngine renderer;
	private readonly ISoundEngine soundEngine;
	private readonly IInputEngine inputEngine;
	private readonly IPhysicsEngine physicsEngine;
	private readonly IGameObjectManager gameObjectManager;

	private readonly GameObject camera, uiCamera;
	private readonly Stopwatch renderStopwatch, updateStopwatch, engineTime;

	public int TickRate { get; set; } = 128;
	public int FpsCap { get; set; } = 240;

	public IntPtr WindowHandle => renderer.WindowHandle;
	public float FpsDeltaTime => (float)renderStopwatch.Elapsed.TotalSeconds;
	public float TickDeltaTime => (float)updateStopwatch.Elapsed.TotalSeconds;
	public float ElapsedSeconds => (float)engineTime.Elapsed.TotalSeconds;

	public GameEngine(ILogger logger, IGraphicsEngine renderer, ISoundEngine soundEngine, IInputEngine inputEngine, IPhysicsEngine physicsEngine, IGameObjectManager gameObjectManager)
	{
		this.logger = logger;
		this.renderer = renderer;
		this.soundEngine = soundEngine;
		this.inputEngine = inputEngine;
		this.physicsEngine = physicsEngine;
		this.gameObjectManager = gameObjectManager;

		renderStopwatch = new Stopwatch();
		updateStopwatch = new Stopwatch();
		engineTime = new Stopwatch();
		engineTime.Start();

		camera = gameObjectManager.CreateGameObject();
		uiCamera = gameObjectManager.CreateGameObject();
	}

	private void SetScene()
	{
		camera.Transform.Position = new Vector3(0, 0, -1);
		camera.Meshes.Add(new MeshData("Camera.obj", ""));
		GameObjectData cameraData = camera.TranslateGameObject();
		renderer.RegisterCameraGameObject(ref cameraData, new ViewPort(0.5f, 0.65f, 1, 0.5f));

		GameObject secondCamera = gameObjectManager.CreateGameObject();
		secondCamera.Transform.Position = new Vector3(0, 5, -10);
		GameObjectData secondCameraData = secondCamera.TranslateGameObject();
		renderer.RegisterCameraGameObject(ref secondCameraData, new ViewPort(0.5f, 0.25f, 1, 0.5f));

		uiCamera.UI = true;
		uiCamera.Transform.Position = new Vector3(0, 0, -1);
		GameObjectData uiCameraData = uiCamera.TranslateGameObject();
		renderer.RegisterCameraGameObject(ref uiCameraData, new ViewPort(0.5f, 0.5f, 1, 1));
	}

	public void Run()
	{
		SetScene();

		new Thread(UpdateLoop).Start(); // Update Thread
		RenderLoop(); // Render Thread
	}

	private void UpdateLoop()
	{
		Thread.CurrentThread.Name = "Update Thread";

		while (true)
		{
			updateStopwatch.Restart();

			// Tick Limit
			int timeToWait = 1000 / TickRate - (int)updateStopwatch.ElapsedMilliseconds;
			Thread.Sleep(timeToWait > 0 ? timeToWait : 0);

			ApplyPhysicsUpdates(physicsEngine.PhysicsPass(TickDeltaTime));
			SyncPhysicsSoundEngines();

			// camera.Transform.Rotation += new Vector3(0, 22.5f *TickDeltaTime, 0);
		}
	}

	private void RenderLoop()
	{
		Thread.CurrentThread.Name = "Render Thread";

		renderer.Start();
		while (true)
		{
			renderStopwatch.Restart();

			renderer.RenderFrame();

			// Fps Limit
			int timeToWait = 1000 / FpsCap - (int)renderStopwatch.ElapsedMilliseconds;
			Thread.Sleep(timeToWait > 0 ? timeToWait : 0);

			SyncRenderEngine();
		}
	}

	private void ApplyPhysicsUpdates(List<PhysicsGameObjectUpdateData> physicsUpdates)
	{
		foreach (PhysicsGameObjectUpdateData physicsUpdate in physicsUpdates)
		{
			GameObject? match = gameObjectManager.GameObjects.Find((gameObject) => gameObject.Id == physicsUpdate.Id);
			match?.Transform.CopyFrom(physicsUpdate.Transform);
		}
	}

	private void SyncPhysicsSoundEngines()
	{
		for (int i = 0; i < gameObjectManager.GameObjects.Count; i++)
		{
			GameObject gameObject = gameObjectManager.GameObjects[i];
			if (!gameObject.SyncGraphics)
				continue;

			GameObjectData comGameObject = gameObject.TranslateGameObject();

			// Physics Engine
			if (gameObject.SyncPhysics)
			{
				if (!gameObject.RegisteredPhysics)
				{
					physicsEngine.RegisterPhysicsObject(ref comGameObject);
					gameObject.RegisteredPhysics = true;
				}
				physicsEngine.UpdatePhysicsObjectForces(ref comGameObject);
			}

			// Sound Engine
			if (gameObject.SyncSound)
			{
				if (!gameObject.RegisteredSound)
				{
					// TODO: update sound objects
					// soundEngine.RegisterSoundObject(ref comGameObject);
					gameObject.RegisteredSound = true;
				}
			}

			gameObject.SyncPhysics = false;
			gameObject.SyncSound = false;
			gameObject.TransformDirty = true;
		}
	}

	private void SyncRenderEngine()
	{
		for (int i = 0; i < gameObjectManager.GameObjects.Count; i++)
		{
			GameObject gameObject = gameObjectManager.GameObjects[i];
			if (!gameObject.SyncGraphics)
				continue;

			GameObjectData comGameObject = gameObject.TranslateGameObject();

			// Graphics Engine
			if (!gameObject.RegisteredGraphics)
			{
				renderer.RegisterObject(ref comGameObject);
				gameObject.RegisteredGraphics = true;
			}

			renderer.UpdateObject(ref comGameObject);

			gameObject.SyncGraphics = false;
		}
	}
}