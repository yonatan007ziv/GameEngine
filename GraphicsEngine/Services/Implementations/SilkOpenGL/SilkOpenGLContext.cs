using Silk.NET.OpenGL;

namespace GraphicsEngine.Services.Implementations.SilkOpenGL;

internal class SilkOpenGLContext
{
	private static bool _isInitialized;
	public static SilkOpenGLContext _instance = null!;
	public static SilkOpenGLContext Instance { get => _instance; set { if (!_isInitialized) { _instance = value; _isInitialized = true; } } }

	public readonly GL silkOpenGLContext;

	public SilkOpenGLContext(GL silkOpenGLContext)
	{
		this.silkOpenGLContext = silkOpenGLContext;
	}
}