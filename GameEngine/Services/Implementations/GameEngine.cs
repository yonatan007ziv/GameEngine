using GameEngine.Components;
using GameEngine.Components.ScriptableObjects;
using GameEngine.Core.API;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;
using GameEngine.Core.SharedServices.Interfaces;
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

	private readonly Dictionary<int, WorldObject> worldCameras = new Dictionary<int, WorldObject>();
	private readonly Dictionary<int, UIObject> uiCameras = new Dictionary<int, UIObject>();

	private readonly Stopwatch renderStopwatch, updateStopwatch, engineTime;
	private readonly int ExpectedTaskSchedulerPeriod;

	private bool screenSizeChanged;
	private Vector2 newScreenSize;

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

	private bool mouseLocked, updateMouseLocked;
	public bool MouseLocked { get => mouseLocked; set { mouseLocked = value; updateMouseLocked = true; } }

	public IntPtr WindowHandle => GraphicsEngine.WindowHandle;
	public float FpsDeltaTimeStopper => (float)renderStopwatch.Elapsed.TotalSeconds;
	public float TickDeltaTimeStopper => (float)updateStopwatch.Elapsed.TotalSeconds;
	public float ElapsedSeconds => (float)engineTime.Elapsed.TotalSeconds;

	public TaskCompletionSource EngineLoadingTask { get; }

	public GameEngine(ILogger logger, IResourceDiscoverer resourceDiscoverer, IGraphicsEngine renderer, ISoundEngine soundEngine, IInputEngine inputEngine, IPhysicsEngine physicsEngine)
	{
		this.logger = logger;
		this.resourceDiscoverer = resourceDiscoverer;

		resourceDiscoverer.AddResourceFolder(Directory.GetCurrentDirectory() + @"\EngineResources");

		GraphicsEngine = renderer;
		SoundEngine = soundEngine;
		InputEngine = inputEngine;
		PhysicsEngine = physicsEngine;

		EngineContext = this;

		EngineLoadingTask = new TaskCompletionSource();
		GraphicsEngine.Load += EngineLoadingTask.SetResult;
		GraphicsEngine.ScreenSizeChanged += (vec) => { screenSizeChanged = true; newScreenSize = vec; };

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

	private void UpdateLoop()
	{
		Thread.CurrentThread.Name = "Update Thread";

		float TickDeltaTime = TickRate / 1000f;
		while (true)
		{
			updateStopwatch.Restart();

			// Tps limit
			double timeToWait = (1000 / TickRate - (int)updateStopwatch.ElapsedMilliseconds) / 1000d;
			AccurateSleep(timeToWait, ExpectedTaskSchedulerPeriod);

			TickDeltaTime = TickDeltaTimeStopper;

			PhysicsEngine.PhysicsTickPass(TickDeltaTime);

			for (int i = 0; i < worldObjects.Keys.Count; i++)
				UpdateWorldObjectTree(worldObjects[worldObjects.Keys.ElementAt(i)], TickDeltaTime);
			for (int i = 0; i < worldCameras.Keys.Count; i++)
				UpdateWorldObjectTree(worldCameras[worldCameras.Keys.ElementAt(i)], TickDeltaTime);

			for (int i = 0; i < uiObjects.Keys.Count; i++)
				UpdateUIObjectTree(uiObjects[uiObjects.Keys.ElementAt(i)], TickDeltaTime);
			for (int i = 0; i < uiCameras.Keys.Count; i++)
				UpdateUIObjectTree(uiCameras[uiCameras.Keys.ElementAt(i)], TickDeltaTime);

			// Reset screen size changed flag
			screenSizeChanged = false;

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

			GraphicsEngine.RenderFrame();

			if (updateMouseLocked)
			{
				GraphicsEngine.SetLockedMouse(mouseLocked);
				updateMouseLocked = false;
			}

			// Fps limit
			double timeToWait = (1000 / FpsCap - (int)renderStopwatch.ElapsedMilliseconds) / 1000d;
			AccurateSleep(timeToWait, ExpectedTaskSchedulerPeriod);

			// Clear buffers
			GraphicsEngine.DeleteFinalizedBuffers();

			FpsDeltaTime = FpsDeltaTimeStopper;

			if (LogFps)
				logger.LogInformation("Fps: {fps}", 1 / FpsDeltaTime);
		}
	}

	private void UpdateWorldObjectTree(WorldObject worldObject, float deltaTime)
	{
		// Update object
		if (worldObject is ScriptableWorldObject scriptableWorldObject)
			scriptableWorldObject.Update(deltaTime);

		// Update components
		for (int i = 0; i < worldObject.Children.Count; i++)
			if (worldObject.Children[i] is WorldObject childWorldObject)
				UpdateWorldObjectTree(childWorldObject, deltaTime);
	}

	private void UpdateUIObjectTree(UIObject uiObject, float deltaTime)
	{
		if (screenSizeChanged)
			uiObject.OnScreenSizeChanged?.Invoke(newScreenSize);

		// Update object
		if (uiObject is ScriptableUIObject scriptableUIObject)
			scriptableUIObject.Update(deltaTime);

		// Update components
		for (int i = 0; i < uiObject.Children.Count; i++)
			if (uiObject.Children[i] is UIObject childUIObject)
				UpdateUIObjectTree(childUIObject, deltaTime);
	}

	public void SetResourceFolder(string path)
		=> resourceDiscoverer.AddResourceFolder(path);

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

		// Add to engines
		PhysicsEngine.AddPhysicsObject(worldObject);
		GraphicsEngine.AddWorldObject(worldObject);

		worldObject.NotifyLoaded();
	}
	public void AddWorldCamera(WorldObject worldCamera, CameraRenderingMask renderingMask, ViewPort viewport)
	{
		if (allObjectIds.Contains(worldCamera.Id))
		{
			logger.LogError("Object id exists");
			return;
		}

		worldCameras.Add(worldCamera.Id, worldCamera);
		allObjectIds.Add(worldCamera.Id);

		// Add to engines
		GraphicsEngine.AddWorldCamera(worldCamera, renderingMask, viewport);

		worldCamera.NotifyLoaded();
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

		// Add to engines
		GraphicsEngine.AddUIObject(uiObject);

		uiObject.NotifyLoaded();
	}
	public void AddUICamera(UIObject uiCamera, CameraRenderingMask renderingMask, ViewPort viewport)
	{
		if (allObjectIds.Contains(uiCamera.Id))
		{
			logger.LogError("Object id exists");
			return;
		}

		uiCameras.Add(uiCamera.Id, uiCamera);
		allObjectIds.Add(uiCamera.Id);

		// Add to engines
		GraphicsEngine.AddUICamera(uiCamera, renderingMask, viewport);

		uiCamera.NotifyLoaded();
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

		// Remove from engines
		PhysicsEngine.RemovePhysicsObject(worldObject);
		GraphicsEngine.RemoveWorldObject(worldObject);

		worldObject.NotifyUnloaded();
	}
	public void RemoveWorldCamera(WorldObject worldCamera)
	{
		if (!allObjectIds.Contains(worldCamera.Id))
		{
			logger.LogError("Object id not found");
			return;
		}

		worldCameras.Remove(worldCamera.Id);
		allObjectIds.Remove(worldCamera.Id);

		// Remove from engines
		GraphicsEngine.RemoveWorldCamera(worldCamera);

		worldCamera.NotifyUnloaded();
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

		// Remove from engines
		GraphicsEngine.RemoveUIObject(uiObject);

		uiObject.NotifyUnloaded();
	}
	public void RemoveUICamera(UIObject uiCamera)
	{
		if (!allObjectIds.Contains(uiCamera.Id))
		{
			logger.LogError("Object id not found");
			return;
		}

		uiCameras.Remove(uiCamera.Id);
		allObjectIds.Remove(uiCamera.Id);

		GraphicsEngine.RemoveUICamera(uiCamera);

		uiCamera.NotifyUnloaded();
	}
	#endregion

	private void AttachInput()
	{
		GraphicsEngine.MouseEvent += InputEngine.OnMouseEvent;
		GraphicsEngine.KeyboardEvent += InputEngine.OnKeyboardEvent;
		GraphicsEngine.GamepadEvent += InputEngine.OnGamepadEvent;
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