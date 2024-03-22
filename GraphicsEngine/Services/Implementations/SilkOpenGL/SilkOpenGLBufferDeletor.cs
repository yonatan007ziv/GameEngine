using GraphicsEngine.Services.Interfaces;

namespace GraphicsEngine.Services.Implementations.SilkOpenGL;

internal class SilkOpenGLBufferDeletor : IBufferSpecificDeletor
{
	public void DeleteBuffer(int id)
		=> SilkOpenGLContext.Instance.silkOpenGLContext.DeleteBuffer((uint)id);

	public void DeleteTextureBuffer(int id)
		=> SilkOpenGLContext.Instance.silkOpenGLContext.DeleteTexture((uint)id);

	public void DeleteVertexArrayBuffer(int id)
		=> SilkOpenGLContext.Instance.silkOpenGLContext.DeleteVertexArray((uint)id);
}