using OpenGLRenderer.Components;
using OpenGLRenderer.Models;
using OpenGLRenderer.OpenGL.Meshes;
using OpenGLRenderer.OpenGL;
using OpenGLRenderer.Services.Interfaces.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace OpenGLRenderer.Services.Implementations.OpenGL.Renderer;

internal abstract class BaseRenderer : GameWindow
{
	protected int width;
	protected int height;

	public BaseRenderer(ISettingsManager settingsManager)
        : base(new GameWindowSettings(), new NativeWindowSettings())
    {
		SettingsModel settings = settingsManager.LoadSettings();
		width = settings.ScreenDimensions.X;
		height = settings.ScreenDimensions.Y;
		VSync = settings.VSync ? VSyncMode.On : VSyncMode.Off;
	}

	new protected abstract void RenderFrame(float deltaTime);
	new protected abstract void UpdateFrame(float deltaTime);
	new protected abstract void Load();
	new protected abstract void Unload();

	protected override void OnRenderFrame(FrameEventArgs args)
	{
		GL.ClearColor(1, 1, 1, 0);
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		RenderFrame((float)args.Time);

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
		base.OnResize(e);
		GL.Viewport(0, 0, e.Width, e.Height);

		width = e.Width;
		height = e.Height;
	}

	protected override void OnLoad()
	{
		base.OnLoad();
		GL.Enable(EnableCap.DepthTest);
		Load();
	}

	protected override void OnUnload()
	{
		base.OnUnload();
		Unload();
	}
}