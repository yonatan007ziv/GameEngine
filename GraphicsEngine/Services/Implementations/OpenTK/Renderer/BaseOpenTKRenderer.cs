using GraphicsEngine.Components.Extensions;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;
using System.Numerics;

namespace GraphicsEngine.Services.Implementations.OpenTK.Renderer;

public abstract class BaseOpenTKRenderer : GameWindow
{
    public IntPtr WindowHandle { get { unsafe { return GLFW.GetWin32Window(WindowPtr); }; } }
    public Vector2 WindowSize { get; private set; } = new Vector2(640, 360);
    public new Vector2 MousePosition { get => base.MousePosition.ToNumerics(); set => base.MousePosition = value.ToOpenTK(); }
    public bool LogRenderingMessages { get; set; }

    public event Action? ResizedEvent;

    private bool mouseLocked, mouseLockedUpdate;

    public BaseOpenTKRenderer()
        : base(new GameWindowSettings(), new NativeWindowSettings())
    {
        CenterWindow();
    }

    public new Vector2 Resize()
        => WindowSize;

    protected abstract void ErrorCallback(string msg, DebugSeverity severity);

    public void Start()
    {
        Context.MakeCurrent();
        OnLoad();
    }

    public new void SwapBuffers()
        => Context.SwapBuffers();

    public void LockMouse(bool lockMouse)
    {
        mouseLocked = lockMouse;
        mouseLockedUpdate = true;
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
        ProcessWindowEvents(false);

        if (mouseLockedUpdate)
        {
            CursorState = mouseLocked ? CursorState.Grabbed : CursorState.Normal;
            mouseLockedUpdate = false;
        }

        if (_changedBackgroundColor)
        {
            GL.ClearColor(backgroundColor.R / 255f, backgroundColor.G / 255f, backgroundColor.B / 255f, 1);
            _changedBackgroundColor = false;
        }

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        unsafe
        {
            if (GLFW.WindowShouldClose(WindowPtr))
            {
                Close();
                Environment.Exit(0);
            }
        }
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        WindowSize = new Vector2(e.Width, e.Height);
        ResizedEvent?.Invoke();
    }

    protected new abstract void Load();

    protected override void OnLoad()
    {
        Console.WriteLine("BaseOpenGLRenderer: OnLoad, register DebugCallback");
        GL.Enable(EnableCap.DebugOutput);
        GL.ActiveTexture(TextureUnit.Texture0);
        // GL.DebugMessageCallback(GLDebugCallback, IntPtr.Zero);

        Load();
    }

    //private void GLDebugCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
    //{
    //	if (LogRenderingMessages)
    //	{
    //		string msg = Marshal.PtrToStringAnsi(message, length);
    //		ErrorCallback(msg, severity);
    //	}
    //}
}