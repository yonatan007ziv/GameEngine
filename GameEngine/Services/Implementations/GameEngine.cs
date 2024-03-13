using GameEngine.Components.Objects;
using GameEngine.Components.Objects.Scriptable;
using GameEngine.Components.UIComponents;
using GameEngine.Core.API;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Physics;
using GameEngine.Core.SharedServices.Interfaces;
using GameEngine.Extensions;
using GameEngine.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;

namespace GameEngine.Services.Implementations;

internal class GameEngine : IGameEngine
{
	public static IGameEngine EngineContext { get; private set; } = null!;

	private readonly ILogger logger;
	private readonly IResourceDiscoverer resourceDiscoverer;

	public IGraphicsEngine GraphicsEngine { get; }
	public ISoundEngine SoundEngine { get; }
	public IInputEngine InputEngine { get; }
	public IPhysicsEngine PhysicsEngine { get; }

	private readonly List<int> allObjectIds = new List<int>();

	private readonly Dictionary<int, WorldObject> worldObjects = new Dictionary<int, WorldObject>();
	private readonly Dictionary<int, UIObject> uiObjects = new Dictionary<int, UIObject>();

	private readonly Dictionary<int, WorldComponent> worldCameras = new Dictionary<int, WorldComponent>();
	private readonly Dictionary<int, UIComponent> uiCameras = new Dictionary<int, UIComponent>();

	private readonly Stopwatch renderStopwatch, updateStopwatch, engineTime;
	private readonly int ExpectedTaskSchedulerPeriod;

	public string Title { get => GraphicsEngine.Title; set => GraphicsEngine.Title = value; }

	// Vector2.One limits the position to [-1 : 1] range and Vector2(1, -1) used to flip the y sign
	public Vector2 NormalizedMousePosition => (2 * InputEngine.GetMousePos() / GraphicsEngine.WindowSize - Vector2.One) * new Vector2(1, -1);

	public bool LogRenderingLogs { get => GraphicsEngine.LogRenderingMessages; set => GraphicsEngine.LogRenderingMessages = value; }
	public bool LogInputs { get => InputEngine.LogInputs; set => InputEngine.LogInputs = value; }
	public bool LogFps { get; set; }
	public bool LogTps { get; set; }

	public bool DrawColliderGizmos { get; set; }

	public int TickRate { get; set; } = 128;
	public int FpsCap { get; set; } = 144;

	private bool _mouseLocked, _updateMouseLocked;
	public bool MouseLocked { get => _mouseLocked; set { _mouseLocked = value; _updateMouseLocked = true; } }

	public IntPtr WindowHandle => GraphicsEngine.WindowHandle;
	public float FpsDeltaTimeStopper => (float)renderStopwatch.Elapsed.TotalSeconds;
	public float TickDeltaTimeStopper => (float)updateStopwatch.Elapsed.TotalSeconds;
	public float ElapsedSeconds => (float)engineTime.Elapsed.TotalSeconds;

	public TaskCompletionSource EngineLoadingTask { get; }

	public GameEngine(ILogger logger, IResourceDiscoverer resourceDiscoverer, IGraphicsEngine renderer, ISoundEngine soundEngine, IInputEngine inputEngine, IPhysicsEngine physicsEngine)
	{
		this.logger = logger;
		this.resourceDiscoverer = resourceDiscoverer;

		GraphicsEngine = renderer;
		SoundEngine = soundEngine;
		InputEngine = inputEngine;
		PhysicsEngine = physicsEngine;

		EngineContext = this;

		EngineLoadingTask = new TaskCompletionSource();
		GraphicsEngine.Load += EngineLoadingTask.SetResult;

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

	public void SetResourceFolder(string path)
		=> resourceDiscoverer.InitResourceFolder(path);

	public void SetBackgroundColor(Color color)
		=> GraphicsEngine.SetBackgroundColor(color);

	#region Object management
	public WorldObject? GetWorldObjectFromId(int id)
		=> worldObjects.ContainsKey(id) ? worldObjects[id] : null;
	public UIObject? GetUIObjectFromId(int id)
		=> uiObjects.ContainsKey(id) ? uiObjects[id] : null;

	public void AddWorldObject(WorldObject worldObject)
	{
		if (allObjectIds.Contains(worldObject.Id))
		{
			logger.LogError("Object id exists");
			return;
		}

		worldObjects.Add(worldObject.Id, worldObject);
		allObjectIds.Add(worldObject.Id);

		WorldObjectData worldObjectData = worldObject.TranslateWorldObject();
		PhysicsEngine.AddPhysicsObject(ref worldObjectData);
		GraphicsEngine.AddWorldObject(ref worldObjectData);
	}
	public void AddWorldCamera(WorldComponent worldCamera, ViewPort viewport)
	{
		if (allObjectIds.Contains(worldCamera.Id))
		{
			logger.LogError("Object id exists");
			return;
		}

		worldCameras.Add(worldCamera.Id, worldCamera);
		allObjectIds.Add(worldCamera.Id);

		GameComponentData worldCameraData = worldCamera.TranslateWorldComponent();
		GraphicsEngine.AddWorldCamera(ref worldCameraData, viewport);
	}
	public void AddUIObject(UIObject uiObject)
	{
		if (allObjectIds.Contains(uiObject.Id))
		{
			logger.LogError("Object id exists");
			return;
		}

		uiObjects.Add(uiObject.Id, uiObject);
		allObjectIds.Add(uiObject.Id);

		UIObjectData uiObjectData = uiObject.TranslateUIObject();
		GraphicsEngine.AddUIObject(ref uiObjectData);
	}
	public void AddUICamera(UIComponent uiCamera, ViewPort viewport)
	{
		if (allObjectIds.Contains(uiCamera.Id))
		{
			logger.LogError("Object id exists");
			return;
		}

		uiCameras.Add(uiCamera.Id, uiCamera);
		allObjectIds.Add(uiCamera.Id);

		GameComponentData uiCameraData = uiCamera.TranslateUIComponent();
		GraphicsEngine.AddUICamera(ref uiCameraData, viewport);
	}

	public void RemoveWorldObject(WorldObject worldObject)
	{
		if (!allObjectIds.Contains(worldObject.Id))
		{
			logger.LogError("Object id not found");
			return;
		}

		worldObjects.Remove(worldObject.Id);
		allObjectIds.Remove(worldObject.Id);

		WorldObjectData worldCameraData = worldObject.TranslateWorldObject();
		GraphicsEngine.RemoveWorldObject(ref worldCameraData);
	}
	public void RemoveWorldCamera(WorldComponent worldCamera)
	{
		if (!allObjectIds.Contains(worldCamera.Id))
		{
			logger.LogError("Object id not found");
			return;
		}

		worldCameras.Remove(worldCamera.Id);
		allObjectIds.Remove(worldCamera.Id);

		GameComponentData worldCameraData = worldCamera.TranslateWorldComponent();
		GraphicsEngine.RemoveWorldCamera(ref worldCameraData);
	}
	public void RemoveUIObject(UIObject uiObject)
	{
		if (!allObjectIds.Contains(uiObject.Id))
		{
			logger.LogError("Object id not found");
			return;
		}

		uiObjects.Remove(uiObject.Id);
		allObjectIds.Remove(uiObject.Id);

		UIObjectData worldCameraData = uiObject.TranslateUIObject();
		GraphicsEngine.RemoveUIObject(ref worldCameraData);
	}
	public void RemoveUICamera(UIComponent uiCamera)
	{
		if (!allObjectIds.Contains(uiCamera.Id))
		{
			logger.LogError("Object id not found");
			return;
		}

		uiCameras.Remove(uiCamera.Id);
		allObjectIds.Remove(uiCamera.Id);

		GameComponentData uiCameraData = uiCamera.TranslateUIComponent();
		GraphicsEngine.RemoveUICamera(ref uiCameraData);
	}
	#endregion

	private void AttachInput()
	{
		GraphicsEngine.MouseEvent += InputEngine.OnMouseEvent;
		GraphicsEngine.KeyboardEvent += InputEngine.OnKeyboardEvent;
		GraphicsEngine.GamepadEvent += InputEngine.OnGamepadEvent;
	}

	private void UpdateLoop()
	{
		Thread.CurrentThread.Name = "Update Thread";

		float TickDeltaTime = TickRate / 1000f;
		while (true)
		{
			updateStopwatch.Restart();

			SyncPhysicsSoundEngines();
			PhysicsGameObjectUpdateData[] physicsUpdates = PhysicsEngine.PhysicsPass(TickDeltaTime);

			// Tps Limit
			double timeToWait = (1000 / TickRate - (int)updateStopwatch.ElapsedMilliseconds) / 1000d;
			AccurateSleep(timeToWait, ExpectedTaskSchedulerPeriod);

			TickDeltaTime = TickDeltaTimeStopper;

			// Apply forces and collider constraints
			ApplyPhysicsUpdates(physicsUpdates);

			lock (worldObjects)
			{
				for (int i = 0; i < worldObjects.Keys.Count; i++)
				{
					WorldObject worldObject = worldObjects[worldObjects.Keys.ElementAt(i)];

					if (worldObject.ImpulseVelocitiesDirty)
						worldObject.ImpulseVelocities.Clear(); // Reset impulse velocities

					// Update components
					foreach (WorldComponent worldComponent in worldObject.components)
						if (worldComponent is ScriptableWorldComponent scriptableWorldComponent)
							scriptableWorldComponent.Update(TickDeltaTime);

					// Update object
					if (worldObject is ScriptableWorldObject scriptableWorldObject)
						scriptableWorldObject.Update(TickDeltaTime);
				}
			}

			lock (uiObjects)
			{
				for (int i = 0; i < uiObjects.Keys.Count; i++)
				{
					UIObject uiObject = uiObjects[uiObjects.Keys.ElementAt(i)];
					// Update components
					foreach (UIComponent uiComponent in uiObject.components)
						if (uiComponent is ScriptableUIComponent scriptableComponent)
							scriptableComponent.Update(TickDeltaTime);

					// Update object
					if (uiObject is ScriptableUIObject scriptableUIObject)
						scriptableUIObject.Update(TickDeltaTime);
				}
			}

			InputEngine.InputTickPass();

			if (LogTps)
				logger.LogInformation("Tick update second: {tps}", 1 / TickDeltaTime);
		}
	}

	private void RenderLoop()
	{
		Thread.CurrentThread.Name = "Render Thread";

		GraphicsEngine.Run();
		float FpsDeltaTime = FpsCap / 1000f;
		while (true)
		{
			renderStopwatch.Restart();

			SyncRenderEngine();
			GraphicsEngine.RenderFrame();

			// Fps Limit
			double timeToWait = (1000 / FpsCap - (int)renderStopwatch.ElapsedMilliseconds) / 1000d;
			AccurateSleep(timeToWait, ExpectedTaskSchedulerPeriod);

			FpsDeltaTime = FpsDeltaTimeStopper;

			if (LogFps)
				logger.LogInformation("Fps: {fps}", 1 / FpsDeltaTime);
		}
	}

	private void ApplyPhysicsUpdates(PhysicsGameObjectUpdateData[] physicsUpdates)
	{
		foreach (PhysicsGameObjectUpdateData physicsUpdate in physicsUpdates)
		{
			if (!allObjectIds.Contains(physicsUpdate.Id))
				continue;

			WorldObject worldObject = worldObjects[physicsUpdate.Id];
			if (worldObject is not null)
			{
				bool isDirtyBefore = worldObject.TransformDirty;
				worldObject.Transform.Position = physicsUpdate.Transform.position;
				worldObject.TransformDirty = isDirtyBefore;
			}
		}
	}

	private void SyncPhysicsSoundEngines()
	{
		WorldObject[] worldObjects = this.worldObjects.Values.ToArray();
		foreach (WorldObject worldObject in worldObjects)
		{
			if (!worldObject.SyncPhysics && !worldObject.SyncSound)
				continue;

			WorldObjectData comGameObject = worldObject.TranslateWorldObject();

			// Physics Engine
			if (worldObject.SyncPhysics)
				PhysicsEngine.UpdatePhysicsObject(ref comGameObject);

			// Sound Engine
			if (worldObject.SyncSound)
			{ }

			worldObject.ResetPhysicsSoundDirty();
		}
	}

	private void SyncRenderEngine()
	{
		if (_updateMouseLocked)
		{
			GraphicsEngine.LockMouse(_mouseLocked);
			_updateMouseLocked = false;
		}

		WorldObject[] worldObjects = this.worldObjects.Values.ToArray();
		foreach (WorldObject worldObject in worldObjects)
		{
			if (!worldObject.SyncGraphics)
				continue;

			WorldObjectData worldObjectData = worldObject.TranslateWorldObject();

			// Graphics Engine
			GraphicsEngine.UpdateWorldObject(ref worldObjectData);

			worldObject.SyncGraphics = false;
		}

		UIObject[] uiObjects = this.uiObjects.Values.ToArray();
		foreach (UIObject uiObject in uiObjects)
		{
			if (!uiObject.SyncGraphics)
				continue;

			UIObjectData uiObjectData = uiObject.TranslateUIObject();

			// Graphics Engine
			GraphicsEngine.UpdateUIObject(ref uiObjectData);

			uiObject.SyncGraphics = false;
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