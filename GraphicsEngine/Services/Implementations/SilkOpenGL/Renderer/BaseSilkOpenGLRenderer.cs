using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;

namespace GraphicsEngine.Services.Implementations.SilkOpenGL.Renderer;

internal abstract class BaseSilkOpenGLRenderer
{
	protected readonly IWindow window;

	public GL openGLContext { get; private set; }
	public string Title { get => window.Title; set => window.Title = value; }
	public bool LogRenderingMessages { get; set; }
	public IntPtr WindowHandle => window.Handle;
	public Vector2 WindowSize { get => (Vector2)window.Size; }

	// Window dimensions
	protected int Width => window.Size.X;
	protected int Height => window.Size.Y;

	public BaseSilkOpenGLRenderer()
	{
		window = Window.Create(WindowOptions.Default);
		window.Load += InternalLoad;
		window.VSync = false;

		openGLContext = null!;
	}

	protected abstract void ErrorCallback(string msg, GLEnum severity);

	protected unsafe virtual void InternalLoad()
	{
		openGLContext = GL.GetApi(window);
		openGLContext.Enable(EnableCap.DebugOutput);
		openGLContext.ActiveTexture(TextureUnit.Texture0);

		unsafe
		{
			openGLContext.DebugMessageCallback(GLDebugCallback, null);
		}

		Load();
	}

	protected abstract void Load();

	public void Start()
	{
		window.Initialize();
		window.MakeCurrent();
	}

	#region Background color
	private bool _changedBackgroundColor;
	private Color backgroundColor;
	public void SetBackgroundColor(Color color)
	{
		_changedBackgroundColor = true;
		backgroundColor = color;
	}
	#endregion

	public void ProcessEvents()
	{
		if (window.IsClosing)
			Environment.Exit(0);

		window.DoEvents();

		if (_changedBackgroundColor)
		{
			openGLContext.ClearColor(backgroundColor.R / 255f, backgroundColor.G / 255f, backgroundColor.B / 255f, 1);
			_changedBackgroundColor = false;
		}

		openGLContext.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
	}

	public void SwapBuffers()
	{
		window.SwapBuffers();
	}

	private void GLDebugCallback(GLEnum source, GLEnum type, int id, GLEnum severity, int length, nint message, nint userParam)
	{
		if (LogRenderingMessages)
		{
			string msg = Marshal.PtrToStringAnsi(message, length);
			ErrorCallback(msg, severity);
		}
	}
}