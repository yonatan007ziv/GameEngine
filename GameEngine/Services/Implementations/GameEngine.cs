using GameEngine.Components;
using GameEngine.Core.API;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Buttons;
using GameEngine.Core.Components.Physics;
using GameEngine.Extensions;
using GameEngine.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace GameEngine.Services.Implementations;

internal class GameEngine : IGameEngine
{
	public static IGameEngine EngineContext { get; private set; } = null!;

	private readonly ILogger logger;

	public IGraphicsEngine Renderer { get; }
	public ISoundEngine SoundEngine { get; }
	public IInputEngine InputEngine { get; }
	public IPhysicsEngine PhysicsEngine { get; }


	private readonly Dictionary<int, GameObject> allObjects = new Dictionary<int, GameObject>();
	private readonly Dictionary<int, GameObject> worldObjects = new Dictionary<int, GameObject>();
	private readonly Dictionary<int, GameObject> uiObjects = new Dictionary<int, GameObject>();

	private readonly Dictionary<int, GameComponent> allCameras = new Dictionary<int, GameComponent>();
	private readonly Dictionary<int, GameComponent> worldCameras = new Dictionary<int, GameComponent>();
	private readonly Dictionary<int, GameComponent> uiCameras = new Dictionary<int, GameComponent>();

	private readonly Stopwatch renderStopwatch, updateStopwatch, engineTime;
	private readonly int ExpectedTaskSchedulerPeriod;

	public string Title { get => Renderer.Title; set => Renderer.Title = value; }

	public bool LogRenderingLogs { get => Renderer.LogRenderingMessages; set => Renderer.LogRenderingMessages = value; }
	public bool LogInputs { get => InputEngine.LogInputs; set => InputEngine.LogInputs = value; }
	public bool LogFps { get; set; }
	public bool LogTps { get; set; }

	public int TickRate { get; set; } = 128;
	public int FpsCap { get; set; } = 144;

	private bool _mouseLocked, _updateMouseLocked;
	public bool MouseLocked { get => _mouseLocked; set { _mouseLocked = value; _updateMouseLocked = true; } }

	public IntPtr WindowHandle => Renderer.WindowHandle;
	public float FpsDeltaTimeStopper => (float)renderStopwatch.Elapsed.TotalSeconds;
	public float TickDeltaTimeStopper => (float)updateStopwatch.Elapsed.TotalSeconds;
	public float ElapsedSeconds => (float)engineTime.Elapsed.TotalSeconds;

	public TaskCompletionSource EngineLoadingTask { get; }

	public GameEngine(ILogger logger, IGraphicsEngine renderer, ISoundEngine soundEngine, IInputEngine inputEngine, IPhysicsEngine physicsEngine)
	{
		this.logger = logger;
		Renderer = renderer;
		SoundEngine = soundEngine;
		InputEngine = inputEngine;
		PhysicsEngine = physicsEngine;

		EngineContext = this;

		EngineLoadingTask = new TaskCompletionSource();
		Renderer.Load += EngineLoadingTask.SetResult;

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
	}

	public void Run()
	{
		new Thread(UpdateLoop).Start(); // Update Thread;
		RenderLoop(); // Render Thread
	}

	public void SetBackgroundColor(Color color)
		=> Renderer.SetBackgroundColor(color);

	public void AddCamera(GameComponent gameComponent, ViewPort viewport)
	{
		if (allCameras.ContainsKey(gameComponent.Id))
		{
			logger.LogError("GameEngine. Object id exists");
			return;
		}

		if (!gameComponent.IsUI)
			worldCameras.Add(gameComponent.Id, gameComponent);
		else
			uiCameras.Add(gameComponent.Id, gameComponent);
		allCameras.Add(gameComponent.Id, gameComponent);

		GameComponentData comGameObject = gameComponent.TranslateGameComponent();
		Renderer.AddCamera(ref comGameObject, viewport);
	}

	public void RemoveCamera(GameComponent gameComponent)
	{
		if (!allCameras.ContainsKey(gameComponent.Id))
		{
			logger.LogError("GameEngine. Camera id doesn't exist");
			return;
		}

		if (gameComponent.IsUI)
			uiCameras.Remove(gameComponent.Id);
		else
			worldCameras.Remove(gameComponent.Id);
		allCameras.Remove(gameComponent.Id);

		GameComponentData comGameObject = gameComponent.TranslateGameComponent();
		Renderer.RemoveCamera(ref comGameObject);
	}

	public void AddGameObject(GameObject gameObject)
	{
		if (allObjects.ContainsKey(gameObject.Id))
		{
			logger.LogError("GameEngine. Object id is taken");
			return;
		}

		if (!gameObject.IsUI)
			worldObjects.Add(gameObject.Id, gameObject);
		else
			uiObjects.Add(gameObject.Id, gameObject);
		allObjects.Add(gameObject.Id, gameObject);

		GameObjectData comGameObject = gameObject.TranslateGameObject();
		PhysicsEngine.AddPhysicsObject(ref comGameObject);
		Renderer.AddGameObject(ref comGameObject);
	}

	public void RemoveGameObject(GameObject gameObject)
	{
		if (!allObjects.ContainsKey(gameObject.Id))
		{
			logger.LogError("GameEngine: Object id not found");
			return;
		}

		if (gameObject.IsUI)
			uiObjects.Remove(gameObject.Id);
		else
			worldObjects.Remove(gameObject.Id);
		allObjects.Remove(gameObject.Id);

		GameObjectData comGameObject = gameObject.TranslateGameObject();
		PhysicsEngine.RemovePhysicsObject(ref comGameObject);
		Renderer.RemoveGameObject(ref comGameObject);
	}

	public bool IsMouseButtonPressed(MouseButton mouseButton)
		=> InputEngine.GetMouseButtonPressed(mouseButton);

	public bool IsMouseButtonDown(MouseButton mouseButton)
		=> InputEngine.GetMouseButtonDown(mouseButton);

	public bool IsKeyboardButtonPressed(KeyboardButton keyboardButton)
		=> InputEngine.GetKeyboardButtonPressed(keyboardButton);

	public bool IsKeyboardButtonDown(KeyboardButton keyboardButton)
		=> InputEngine.GetKeyboardButtonDown(keyboardButton);

	private void AttachInput()
	{
		Renderer.MouseEvent += InputEngine.OnMouseEvent;
		Renderer.KeyboardEvent += InputEngine.OnKeyboardEvent;
		Renderer.GamepadEvent += InputEngine.OnGamepadEvent;
	}

	private void UpdateLoop()
	{
		Thread.CurrentThread.Name = "Update Thread";

		float TickDeltaTime = TickRate / 1000f;
		while (true)
		{
			updateStopwatch.Restart();

			SyncPhysicsSoundEngines();
			ApplyPhysicsUpdates(PhysicsEngine.PhysicsPass(TickDeltaTime));

			// Tps Limit
			double timeToWait = (1000 / TickRate - (int)updateStopwatch.ElapsedMilliseconds) / 1000d;
			AccurateSleep(timeToWait, ExpectedTaskSchedulerPeriod);

			TickDeltaTime = TickDeltaTimeStopper;

			// Checking for input after limiting TPS
			GameObject[] gameObjects = allObjects.Values.ToArray();
			foreach (GameObject gameObject in gameObjects)
			{
				if (gameObject.ImpulseVelocitiesDirty)
					gameObject.ImpulseVelocities.Clear(); // Reset impulse velocities

				// Poll scripting code from components
				foreach (GameComponent gameComponent in gameObject.gameComponents)
					if (gameComponent is ScriptableGameComponent scriptableComponent)
						scriptableComponent.Update(TickDeltaTime);

				// Poll scripting code from object
				if (gameObject is ScriptableGameObject scriptableObject)
					scriptableObject.Update(TickDeltaTime);
			}

			InputEngine.InputTickPass();

			if (LogTps)
				logger.LogInformation("Tick update second: {tps}", 1 / TickDeltaTime);
		}
	}

	private void RenderLoop()
	{
		Thread.CurrentThread.Name = "Render Thread";

		Renderer.Start();
		float FpsDeltaTime = FpsCap / 1000f;
		while (true)
		{
			renderStopwatch.Restart();

			SyncRenderEngine();
			Renderer.RenderFrame();

			// Fps Limit
			double timeToWait = (1000 / FpsCap - (int)renderStopwatch.ElapsedMilliseconds) / 1000d;
			AccurateSleep(timeToWait, ExpectedTaskSchedulerPeriod);

			FpsDeltaTime = this.FpsDeltaTimeStopper;

			if (LogFps)
				logger.LogInformation("Fps: {fps}", 1 / FpsDeltaTime);
		}
	}

	private void ApplyPhysicsUpdates(List<PhysicsGameObjectUpdateData> physicsUpdates)
	{
		foreach (PhysicsGameObjectUpdateData physicsUpdate in physicsUpdates)
		{
			GameObject? match = allObjects.ContainsKey(physicsUpdate.Id) ? allObjects[physicsUpdate.Id] : null;
			if (match is not null)
				match.Transform.Position = physicsUpdate.Transform.position;
		}
	}

	private void SyncPhysicsSoundEngines()
	{
		GameObject[] gameObjects = allObjects.Values.ToArray();
		foreach (GameObject gameObject in gameObjects)
		{
			if (!gameObject.SyncPhysics && !gameObject.SyncSound)
				continue;

			GameObjectData comGameObject = gameObject.TranslateGameObject();

			// Physics Engine
			if (gameObject.SyncPhysics)
				PhysicsEngine.UpdatePhysicsObject(ref comGameObject);

			// Sound Engine
			if (gameObject.SyncSound)
			{ }

			gameObject.ResetPhysicsSoundDirty();
		}
	}

	private void SyncRenderEngine()
	{
		if (_updateMouseLocked)
		{
			Renderer.LockMouse(_mouseLocked);
			_updateMouseLocked = false;
		}

		foreach (GameObject gameObject in allObjects.Values)
		{
			if (!gameObject.SyncGraphics)
				continue;

			GameObjectData comGameObject = gameObject.TranslateGameObject();

			// Graphics Engine
			Renderer.UpdateObject(ref comGameObject);

			gameObject.SyncGraphics = false;
		}
	}

	private static void AccurateSleep(double seconds, int expectedSchedulerPeriod)
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