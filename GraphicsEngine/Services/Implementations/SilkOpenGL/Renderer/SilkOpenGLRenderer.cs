using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Events;
using GameEngine.Core.SharedServices.Interfaces;
using GraphicsEngine.Components.Interfaces;
using GraphicsEngine.Components.RendererSpecific.SilkOpenGL.Input;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Components.Shared.Data;
using GraphicsEngine.Services.Implementations.Shared;
using GraphicsEngine.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using System.Numerics;

namespace GraphicsEngine.Services.Implementations.SilkOpenGL.Renderer;

internal class SilkOpenGLRenderer : BaseSilkOpenGLRenderer, IInternalGraphicsRenderer
{
	private readonly ILogger logger;

	public IFactory<string, string, IMeshRenderer> MeshFactory { get; private set; }

	private readonly IFactory<string, ModelData> modelFactory;
	private readonly IFactory<string, Material> materialFactory;

	private IMouse mainMouse;
	public Vector2 MousePosition { get => mainMouse.Position; set => mainMouse.Position = value; }

	public event Action? LoadEvent;
	public event Action? ResizedEvent;
	public event Action<MouseEventData>? MouseEvent;
	public event Action<KeyboardEventData>? KeyboardEvent;
	public event Action<GamepadEventData>? GamepadEvent;

	public SilkOpenGLRenderer(ILogger logger, IFactory<string, ModelData> modelFactory, IFactory<string, Material> materialFactory)
	{
		this.logger = logger;
		this.modelFactory = modelFactory;
		this.materialFactory = materialFactory;

		window.Resize += (size) => { ResizedEvent?.Invoke(); };
		MeshFactory = null!;
	}

	protected override void Load()
	{
		SilkOpenGLContext.Instance = new SilkOpenGLContext(openGLContext);
		MeshFactory = new MeshRendererFactory(logger, new SilkOpenGLDrawingCall(), modelFactory, materialFactory);
		AttachInput();

		LoadEvent?.Invoke();
	}

	private readonly Dictionary<IInputDevice, IInputHandler> inputDevices = new Dictionary<IInputDevice, IInputHandler>();
	private void AttachInput()
	{
		IInputContext context = window.CreateInput();

		// Attach Mouse
		mainMouse = context.Mice[0];
		foreach (IMouse mouse in context.Mice)
			inputDevices.Add(mouse, new SilkOpenGLMouseHandler(mouse, MouseEvent));

		// Attach Keyboard
		foreach (IKeyboard keyboard in context.Keyboards)
			inputDevices.Add(keyboard, new SilkOpenGLKeyboardHandler(keyboard, KeyboardEvent));
		// Attach Joystick
		foreach (IGamepad gamepad in context.Gamepads)
			inputDevices.Add(gamepad, new SilkOpenGLGamepadHandler(gamepad, GamepadEvent));

		context.ConnectionChanged += (inputDevice, connected) =>
		{
			if (inputDevices.ContainsKey(inputDevice))
			{
				if (!connected)
				{
					logger.LogInformation("Disconnected input device: {deviceName}", inputDevices[inputDevice].Name);
					inputDevices[inputDevice].Disconnect();
				}
			}
			else if (connected)
			{
				if (inputDevice is IMouse mouse)
					inputDevices.Add(mouse, new SilkOpenGLMouseHandler(mouse, MouseEvent));
				else if (inputDevice is IKeyboard keyboard)
					inputDevices.Add(keyboard, new SilkOpenGLKeyboardHandler(keyboard, KeyboardEvent));
				else if (inputDevice is IGamepad gamepad)
					inputDevices.Add(gamepad, new SilkOpenGLGamepadHandler(gamepad, GamepadEvent));
				else
					logger.LogInformation("Unsupported input device connected");
			}
		};
	}

	public void SetMouseLocked(bool locked)
	{
		foreach (IInputDevice inputDevice in inputDevices.Keys)
			if (inputDevice is IMouse mouse)
				mouse.Cursor.CursorMode = locked ? CursorMode.Disabled : CursorMode.Normal;
	}

	public void SetDepthTest(bool enable)
	{
		if (enable)
			openGLContext.Enable(EnableCap.DepthTest);
		else
			openGLContext.Disable(EnableCap.DepthTest);
	}

	public void SetViewport(ViewPort viewport)
	{
		openGLContext.Viewport(
			(int)((viewport.x - viewport.width / 2) * WindowSize.X),
			(int)((viewport.y - viewport.height / 2) * WindowSize.Y),
			(uint)(viewport.width * WindowSize.X),
			(uint)(viewport.height * WindowSize.Y)
			);
	}

	protected override void ErrorCallback(string msg, GLEnum severity)
	{
		switch (severity)
		{
			case GLEnum.DebugSeverityLow:
				logger.LogInformation("GL-INFO: {errorMsg}", msg);
				break;
			case GLEnum.DebugSeverityMedium:
				logger.LogError("GL-ERROR: {errorMsg}", msg);
				break;
			case GLEnum.DebugSeverityHigh:
				logger.LogCritical("GL-CRITICAL: {errorMsg}", msg);
				break;
			case GLEnum.DebugSeverityNotification:
				logger.LogTrace("GL-TRACE: {errorMsg}", msg);
				break;
		}
	}
}
