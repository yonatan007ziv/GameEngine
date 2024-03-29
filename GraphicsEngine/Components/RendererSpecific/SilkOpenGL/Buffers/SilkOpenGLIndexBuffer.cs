using GraphicsEngine.Components.Interfaces.Buffers;
using Silk.NET.OpenGL;

namespace GraphicsEngine.Components.RendererSpecific.SilkOpenGL.Buffers;

internal class SilkOpenGLIndexBuffer : SilkOpenGLBuffer, IIndexBuffer
{
	public SilkOpenGLIndexBuffer(GL glContext)
		: base(BufferTargetARB.ElementArrayBuffer, glContext)
	{

	}

	public void WriteData(uint[] indexes)
		=> WriteBuffer(indexes, BufferUsageARB.StaticDraw);
}