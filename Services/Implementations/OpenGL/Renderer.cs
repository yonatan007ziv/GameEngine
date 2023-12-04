using OpenGLRenderer.Components;
using OpenGLRenderer.Models;
using OpenGLRenderer.OpenGL;
using OpenGLRenderer.OpenGL.Meshes;
using OpenGLRenderer.Services.Interfaces.OpenGL;
using OpenGLRenderer.Services.Interfaces.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenGLRenderer.Services.Implementations.OpenGL;

internal class Renderer : GameWindow
{
	private readonly ISettingsManager settingsManager;
	private readonly IModelImporter importer;

	private int width;
	private int height;

	// Temps
	private float yAngle;
	private Camera camera;
	private GameObject gameObject;
	private ShaderProgram sP;

	public Renderer(ISettingsManager settingsManager, IModelImporter importer)
		: base(new GameWindowSettings(), new NativeWindowSettings())
	{
		this.settingsManager = settingsManager;
		this.importer = importer;

		SettingsModel settings = settingsManager.LoadSettings();
		
		width = settings.ScreenDimensions.X;
		height = settings.ScreenDimensions.Y;
		VSync = settings.VSync ? VSyncMode.On : VSyncMode.Off;

		CenterWindow(new Vector2i(width, height));
	}

	protected override void OnRenderFrame(FrameEventArgs args)
	{
		GL.ClearColor(1, 1, 1, 0);
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		float sin = (float)Math.Sin(2.5 * GLFW.GetTime()) / 2 + .5f;
		float cos = (float)Math.Cos(2.5 * GLFW.GetTime()) / 2 + .5f;
		int cL = GL.GetUniformLocation(sP.Id, "Color");
		GL.Uniform3(cL, sin, cos, sin);

		Matrix4 model = Matrix4.CreateRotationY(yAngle += (float)args.Time);
		int modelLoc = GL.GetUniformLocation(sP.Id, "model");
		GL.UniformMatrix4(modelLoc, true, ref model);

		Matrix4 view = camera.ViewMatrix;
		int viewLoc = GL.GetUniformLocation(sP.Id, "view");
		GL.UniformMatrix4(viewLoc, true, ref view);

		Matrix4 projection = camera.ProjectionMatrix;
		int projectionLoc = GL.GetUniformLocation(sP.Id, "projection");
		GL.UniformMatrix4(projectionLoc, true, ref projection);

		gameObject.Draw();

		Context.SwapBuffers();
		base.OnRenderFrame(args);
	}

	protected override void OnUpdateFrame(FrameEventArgs args)
	{
		base.OnUpdateFrame(args);

		camera.Update(MouseState, KeyboardState, args);
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

		sP = new ShaderProgram(new ShaderSource("DefVertex.glsl"), new ShaderSource("DefFragment.glsl"));
		gameObject = new GameObject(Vector3.Zero);
		_ = new CustomMesh(gameObject, importer.ImportModel(@"D:\Code\VS Community\OpenGLRenderer\Resources\Pyramid.obj"), sP);

		camera = new Camera(gameObject, 10, 10, width, height);
	}

	protected override void OnUnload()
	{
		base.OnUnload();
	}
}