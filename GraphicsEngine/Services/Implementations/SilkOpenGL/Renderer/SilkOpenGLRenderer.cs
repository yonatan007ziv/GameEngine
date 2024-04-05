using GameEngine.Core.Components;
using GameEngine.Core.Components.Fonts;
using GameEngine.Core.Components.Input.Events;
using GameEngine.Core.SharedServices.Interfaces;
using GraphicsEngine.Components.Interfaces;
using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.RendererSpecific.SilkOpenGL.Input;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Components.Shared.Data;
using GraphicsEngine.Services.Implementations.Shared;
using GraphicsEngine.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using System.Drawing;
using System.Numerics;

namespace GraphicsEngine.Services.Implementations.SilkOpenGL.Renderer;

internal class SilkOpenGLRenderer : BaseSilkOpenGLRenderer, IInternalGraphicsRenderer
{
	private readonly ILogger logger;

	public IFactory<string, string, MeshRenderer> MeshFactory { get; private set; }

	private readonly IFactory<string, ModelData> modelFactory;
	private readonly IFactory<string, Material> materialFactory;

	private IMouse mainMouse;
	public Vector2 MousePosition { get => mainMouse.Position; set => mainMouse.Position = value; }

	public event Action? LoadEvent;
	public event Action? ResizedEvent;
	public event Action<MouseEventData>? MouseEvent;
	public event Action<KeyboardEventData>? KeyboardEvent;
	public event Action<GamepadEventData>? GamepadEvent;

	public SilkOpenGLRenderer(ILogger logger, IFactory<string, ModelData> modelFactory, IFactory<string, Material> materialFactory, IBufferFactory bufferFactory)
	{
		this.logger = logger;
		this.modelFactory = modelFactory;
		this.materialFactory = materialFactory;

		window.Resize += (size) => { ResizedEvent?.Invoke(); };

		mainMouse = null!;
		MeshFactory = null!;
	}

	protected override void Load()
	{
		SilkOpenGLContext.Instance = new SilkOpenGLContext(OpenGLContext);
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

	public void SetMousePosition(Vector2 position)
	{
		MousePosition = position;
	}

	public void SetDepthTest(bool enable)
	{
		if (enable)
			OpenGLContext.Enable(EnableCap.DepthTest);
		else
			OpenGLContext.Disable(EnableCap.DepthTest);
	}

	public void SetViewport(ViewPort viewport)
	{
		OpenGLContext.Viewport(
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

	public void DrawGlyphs(IEnumerable<(CharacterGlyf glyph, Vector2 position, float fontSize, Color textColor)> glyphData, IVertexBuffer vertexBuffer, IIndexBuffer indexBuffer, IVertexArray vertexArray, IShaderProgram textShader)
	{
		bool prevLogRenderingMessages = LogRenderingMessages;

		LogRenderingMessages = false;
		for (int i = 0; i < glyphData.Count(); i++)
		{
			(CharacterGlyf glyph, Vector2 position, float fontSize, Color textColor) = glyphData.ElementAt(i);
			glyph.FontSize = fontSize;
			textShader.SetFloat4Uniform(new Vector4((float)textColor.R / 0xFF, (float)textColor.G / 0xFF, (float)textColor.B / 0xFF, 1), "textColor");

			foreach (CharacterContour contour in glyph.CharacterContours)
			{
				if (contour.Clockwise)
					OpenGLContext.FrontFace(FrontFaceDirection.CW);
				else
					OpenGLContext.FrontFace(FrontFaceDirection.Ccw);

				Vector2[] points = contour.Points.ToArray();
				for (int j = 0; j < points.Length; j++)
					points[j] += position;

				vertexBuffer.WriteData(points);

				vertexArray.Bind();
				OpenGLContext.DrawArrays(PrimitiveType.LineLoop, 0, (uint)contour.Points.Count);
			}
		}

		LogRenderingMessages = prevLogRenderingMessages;
	}
}