using GraphicsRenderer.Components.Shared.Data;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Runtime.InteropServices;

namespace GraphicsRenderer.Services.Implementations.OpenGL.Renderer;

public abstract class BaseOpenGLRenderer : GameWindow
{
	protected int Width { get; private set; } = 640;
	protected int Height { get; private set; } = 360;

	public BaseOpenGLRenderer()
		: base(new GameWindowSettings(), new NativeWindowSettings())
	{
		CenterWindow();
	}

	public void LockMouse(bool lockMouse)
	{
		CursorState = lockMouse ? CursorState.Grabbed : CursorState.Normal;
	}

	public void TurnVSync(bool vsync)
	{
		VSync = vsync ? VSyncMode.On : VSyncMode.Off;
	}

	new protected abstract void RenderFrame();
	new protected abstract void UpdateFrame(float deltaTime);
	new protected abstract void Load();
	new protected abstract void Unload();
	new protected abstract void Resize(int width, int height);
	protected abstract void GLDebugCallback(string msg, DebugSeverity severity);

	private void GLDebugCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
	{
		string msg = Marshal.PtrToStringAnsi(message, length);
		GLDebugCallback(msg, severity);
	}

	protected override void OnRenderFrame(FrameEventArgs args)
	{
		GL.ClearColor(1, 1, 1, 0);
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		RenderFrame();

		Context.SwapBuffers();
		base.OnRenderFrame(args);
	}

	protected override void OnUpdateFrame(FrameEventArgs args)
	{
		UpdateFrame((float)args.Time);
		base.OnUpdateFrame(args);
	}

	protected override void OnResize(ResizeEventArgs e)
	{
		Resize(e.Width, e.Height);
		base.OnResize(e);
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