using OpenTK.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GraphicsRenderer.Services.Implementations.OpenGL.Renderer;

public abstract class BaseOpenGLRenderer : GameWindow
{
	[DllImport("kernel32")]
	private static extern IntPtr SetThreadAffinityMask(IntPtr hThread, IntPtr dwThreadAffinityMask);

	[DllImport("kernel32")]
	private static extern IntPtr GetCurrentThread();

	[DllImport("winmm")]
	private static extern uint timeBeginPeriod(uint uPeriod);

	[DllImport("winmm")]
	private static extern uint timeEndPeriod(uint uPeriod);

	private readonly Stopwatch updateWatch = new Stopwatch();

	protected int Width { get; private set; } = 640;
	protected int Height { get; private set; } = 360;

	public BaseOpenGLRenderer()
		: base(new GameWindowSettings(), new NativeWindowSettings())
	{
		Title = "GraphicsRenderer";
		CenterWindow();
	}

	public abstract void Render();
	protected new abstract void Load();
	protected new abstract void Unload();
	protected new abstract void Resize(int width, int height);
	protected abstract void GLDebugCallback(string msg, DebugSeverity severity);

	public void Start()
	{
		Context.MakeCurrent();

		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			SetThreadAffinityMask(GetCurrentThread(), new IntPtr(1));
			_ = timeBeginPeriod(8u);
			ExpectedSchedulerPeriod = 8;
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
		{
			_ = timeBeginPeriod(1u);
			ExpectedSchedulerPeriod = 1;
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			_ = timeBeginPeriod(1u);
			ExpectedSchedulerPeriod = 1;
		}

		OnLoad();
		OnResize(new ResizeEventArgs(ClientSize));

		updateWatch.Start();
	}

	public void LockMouse(bool lockMouse)
	{
		CursorState = lockMouse ? CursorState.Grabbed : CursorState.Normal;
	}

	public void TurnVSync(bool vsync)
	{
		VSync = vsync ? VSyncMode.On : VSyncMode.Off;
		UpdateFrequency = vsync ? 60 : 500;
	}

	int frameCount = 0;
	public new void RenderFrame()
	{
		updateWatch.Restart();

		NewInputFrame();
		ProcessWindowEvents(false);

		RenderInternal();

		double timeBetweenFrames = UpdateFrequency == 0 ? 0 : (1.0 / UpdateFrequency);
		double timeToWait = timeBetweenFrames - updateWatch.Elapsed.TotalSeconds;

		if (timeToWait > 0)
			Utils.AccurateSleep(timeToWait, ExpectedSchedulerPeriod);

		unsafe
		{
			if (GLFW.WindowShouldClose(WindowPtr))
			{
				OnUnload();
				Close();
			}
		}
	}

	private void RenderInternal()
	{
		GL.ClearColor(1, 1, 1, 1);
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		Render();
		Context.SwapBuffers();
	}

	private void GLDebugCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
	{
		string msg = Marshal.PtrToStringAnsi(message, length);
		GLDebugCallback(msg, severity);
	}

	protected override void OnResize(ResizeEventArgs e)
	{
		base.OnResize(e);

		Resize(e.Width, e.Height);
		GL.Viewport(0, 0, e.Width, e.Height);

		Width = e.Width;
		Height = e.Height;
	}

	protected override void OnLoad()
	{
		base.OnLoad();

		GL.DebugMessageCallback(GLDebugCallback, IntPtr.Zero);
		GL.Enable(EnableCap.DebugOutput);
		GL.Enable(EnableCap.DepthTest);
		GL.ActiveTexture(TextureUnit.Texture0);
		Load();
	}

	protected override void OnUnload()
	{
		base.OnUnload();
		Unload();
	}
}