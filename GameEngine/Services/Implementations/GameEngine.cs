using GameEngine.Components.GameObjectComponents;
using GameEngine.Core.API;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Input;
using GameEngine.Core.Components.Physics;
using GameEngine.Extensions;
using GameEngine.Services.Interfaces.Managers;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;

namespace GameEngine.Services.Implementations;

internal class GameEngine : IGameEngine
{
	private readonly ILogger logger;
	private readonly IGraphicsEngine renderer;
	private readonly ISoundEngine soundEngine;
	private readonly IInputEngine inputEngine;
	private readonly IPhysicsEngine physicsEngine;
	private readonly IGameObjectManager gameObjectManager;

	private readonly GameObject camera1, camera2, uiCamera;
	private readonly Stopwatch renderStopwatch, updateStopwatch, engineTime;
	private readonly int ExpectedTaskSchedulerPeriod;

	public int TickRate { get; set; } = 128;
	public int FpsCap { get; set; } = 144;

	private bool _mouseLocked;
	public bool MouseLocked { get => _mouseLocked; set { _mouseLocked = value; renderer.LockMouse(value); } }

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

		renderer.Title = "GameEngine";

		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			ExpectedTaskSchedulerPeriod = 8;
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
			ExpectedTaskSchedulerPeriod = 1;
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			ExpectedTaskSchedulerPeriod = 1;

		renderStopwatch = new Stopwatch();
		updateStopwatch = new Stopwatch();
		engineTime = new Stopwatch();
		engineTime.Start();

		AttachInput();

		camera1 = gameObjectManager.CreateGameObject();
		camera2 = gameObjectManager.CreateGameObject();
		uiCamera = gameObjectManager.CreateGameObject();
	}

	private void AttachInput()
	{
		renderer.MousePositionEvent += inputEngine.OnMousePositionEvent;
		renderer.MouseButtonEvent += inputEngine.OnMouseButtonEvent;
		renderer.KeyboardButtonEvent += inputEngine.OnKeyboardButtonEvent;
	}

	private void LoadCamera()
	{
		camera1.Meshes.Add(new MeshData("Camera.obj", "Red.mat"));
		GameObjectData cameraData1 = camera1.TranslateGameObject();
		renderer.RegisterCameraGameObject(ref cameraData1, new ViewPort(0.5f, 0.5f, 1, 1));

		uiCamera.UI = true;
		uiCamera.Transform.Position = new Vector3(0, 0, -1);
		GameObjectData uiCameraData = uiCamera.TranslateGameObject();
		renderer.RegisterCameraGameObject(ref uiCameraData, new ViewPort(0.5f, 0.5f, 1, 1));
	}

	private void Load2Cameras()
	{
		camera1.Meshes.Add(new MeshData("Camera.obj", "Red.mat"));
		GameObjectData cameraData1 = camera1.TranslateGameObject();
		renderer.RegisterCameraGameObject(ref cameraData1, new ViewPort(0.5f, 0.75f, 1, 0.5f));
		
		camera2.Meshes.Add(new MeshData("Camera.obj", "Green.mat"));
		GameObjectData cameraData2 = camera2.TranslateGameObject();
		renderer.RegisterCameraGameObject(ref cameraData2, new ViewPort(0.5f, 0.25f, 1, 0.5f));
		
		uiCamera.UI = true;
		uiCamera.Transform.Position = new Vector3(0, 0, -1);
		GameObjectData uiCameraData = uiCamera.TranslateGameObject();
		renderer.RegisterCameraGameObject(ref uiCameraData, new ViewPort(0.5f, 0.5f, 1, 1));
	}

	private void SetScene()
	{
		GameObject ground = gameObjectManager.CreateGameObject();
		ground.Transform.Scale *= 25;
		ground.Transform.Rotation += Vector3.UnitX * 90;
		ground.Meshes.Add(new MeshData("UIPlane.obj", "Leed.mat"));

		GameObject trex = gameObjectManager.CreateGameObject();
		trex.Transform.Scale /= 5;
		trex.Meshes.Add(new MeshData("Trex.obj", "Hamama.mat"));
	}

	public void Run()
	{
		MouseLocked = true;
		// LoadCamera();
		Load2Cameras();

		SetScene();

		new Thread(UpdateLoop).Start(); // Update Thread;
		RenderLoop(); // Render Thread
	}

	private void UpdateLoop()
	{
		Thread.CurrentThread.Name = "Update Thread";

		float TickDeltaTime = TickRate / 1000f;
		while (true)
		{
			updateStopwatch.Restart();

			ApplyPhysicsUpdates(physicsEngine.PhysicsPass(TickDeltaTime));
			SyncPhysicsSoundEngines();

			int speedFactor = inputEngine.IsKeyboardButtonPressed(KeyboardButton.LShift) ? 5 : 3;

			Vector3 temp = inputEngine.GetMovementVector(KeyboardButton.D, KeyboardButton.A, KeyboardButton.W, KeyboardButton.S);
			camera1.Transform.Position += (-temp.X * camera1.Transform.LocalRight + temp.Z * camera1.Transform.LocalFront) * TickDeltaTime * speedFactor;

			temp = inputEngine.GetMovementVector(KeyboardButton.RightArrow, KeyboardButton.LeftArrow, KeyboardButton.UpArrow, KeyboardButton.DownArrow);
			camera2.Transform.Position += (-temp.X * camera2.Transform.LocalRight + temp.Z * camera2.Transform.LocalFront) * TickDeltaTime * speedFactor;

			Vector2 vec = inputEngine.GetMouseVector();
			camera1.Transform.Rotation += new Vector3(vec.Y, -vec.X, 0) * TickDeltaTime * 45;
			
			camera2.Transform.Rotation += Vector3.UnitY * TickDeltaTime * 45;

			// Tick Limit
			double timeToWait = (1000 / TickRate - (int)updateStopwatch.ElapsedMilliseconds) / 1000d;
			AccurateSleep(timeToWait, ExpectedTaskSchedulerPeriod);

			TickDeltaTime = this.TickDeltaTime;

			if (inputEngine.IsKeyboardButtonDown(KeyboardButton.One))
				MouseLocked = !MouseLocked;
			
			inputEngine.Update();

			logger.LogInformation("Tick update second: {tps}", 1 / TickDeltaTime);
		}
	}

	private void RenderLoop()
	{
		Thread.CurrentThread.Name = "Render Thread";

		renderer.Start();
		float FpsDeltaTime = FpsCap / 1000f;
		while (true)
		{
			renderStopwatch.Restart();

			SyncRenderEngine();
			renderer.RenderFrame();

			// Fps Limit
			double timeToWait = (1000 / FpsCap - (int)renderStopwatch.ElapsedMilliseconds) / 1000d;
			AccurateSleep(timeToWait, ExpectedTaskSchedulerPeriod);

			FpsDeltaTime = this.FpsDeltaTime;

			logger.LogInformation("Frame update second: {fps}", 1 / FpsDeltaTime);
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
			gameObject.TransformDirty = false;
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

	public static void AccurateSleep(double seconds, int expectedSchedulerPeriod)
	{
		if (seconds <= 0)
			return;

		long num = Stopwatch.GetTimestamp() + (long)(seconds * Stopwatch.Frequency);
		int num2 = (int)((seconds * 1000.0 - expectedSchedulerPeriod * 0.02) / expectedSchedulerPeriod);
		
		if (num2 > 0)
			Thread.Sleep(num2 * expectedSchedulerPeriod);

		while (Stopwatch.GetTimestamp() < num)
			Thread.Yield();
	}
}