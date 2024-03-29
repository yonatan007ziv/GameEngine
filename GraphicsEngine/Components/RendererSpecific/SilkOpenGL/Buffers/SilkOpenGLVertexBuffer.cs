using GraphicsEngine.Components.Interfaces.Buffers;
using Silk.NET.OpenGL;

namespace GraphicsEngine.Components.RendererSpecific.SilkOpenGL.Buffers;

internal class SilkOpenGLVertexBuffer : SilkOpenGLBuffer, IVertexBuffer
{
	public SilkOpenGLVertexBuffer(GL glContext)
		: base(BufferTargetARB.ArrayBuffer, glContext)
	{

	}

	public void WriteData<T>(T[] data) where T : struct
		=> WriteBuffer(data, BufferUsageARB.StaticDraw);
}