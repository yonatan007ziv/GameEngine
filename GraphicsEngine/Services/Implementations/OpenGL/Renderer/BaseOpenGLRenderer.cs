using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Runtime.InteropServices;

namespace GraphicsEngine.Services.Implementations.OpenGL.Renderer;

public abstract class BaseOpenGLRenderer : GameWindow
{
	public IntPtr WindowHandle { get { unsafe { return GLFW.GetWin32Window(WindowPtr); }; } }

	protected int Width { get; private set; } = 640;
	protected int Height { get; private set; } = 360;

	private bool mouseLocked, mouseLockedUpdate;

	public BaseOpenGLRenderer()
		: base(new GameWindowSettings(), new NativeWindowSettings())
	{
		Context.MakeNoneCurrent();

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

		OnLoad();
		OnResize(new ResizeEventArgs(ClientSize));
	}

	public void LockMouse(bool lockMouse)
	{
		mouseLocked = lockMouse;
		mouseLockedUpdate = true;
	}

	public new void RenderFrame()
	{
		ProcessWindowEvents(false);

		if (mouseLockedUpdate)
		{
			CursorState = mouseLocked ? CursorState.Grabbed : CursorState.Normal;
			mouseLockedUpdate = false;
		}

		RenderInternal();

		unsafe
		{
			if (GLFW.WindowShouldClose(WindowPtr))
			{
				OnUnload();
				Close();
				Environment.Exit(0);
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

	private void GLDebugCallback(DebugSeverity severity, int length, IntPtr message)
	{
		string msg = Marshal.PtrToStringAnsi(message, length);
		GLDebugCallback(msg, severity);
	}

	protected override void OnResize(ResizeEventArgs e)
	{
		base.OnResize(e);
		Resize(e.Width, e.Height);

		Width = e.Width;
		Height = e.Height;
	}

	private new void OnLoad()
	{
		Console.WriteLine("BaseOpenGLRenderer: OnLoad, register DebugCallback");
		// GL.DebugMessageCallback((DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam) => GLDebugCallback(severity, length, message), IntPtr.Zero);
		GL.Enable(EnableCap.DebugOutput);
		GL.ActiveTexture(TextureUnit.Texture0);
		Load();
	}

	private new void OnUnload()
	{
		Unload();
	}

	public void SetKeyboardCallback(Action<char> action)
	{
		unsafe
		{
			GLFW.SetKeyCallback(WindowPtr, (wnd, key, scanCode, act, mods) => action(key.ToString()[0]));
		}
	}
}