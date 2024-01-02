using GraphicsRenderer.Components.Interfaces.Buffers;
using OpenTK.Graphics.OpenGL4;

namespace GraphicsRenderer.Components.OpenGL.Buffers;

public class OpenGLVertexBuffer : OpenGLBuffer, IVertexBuffer
{

	public OpenGLVertexBuffer()
		: base(BufferTarget.ArrayBuffer)
	{

	}

	public void WriteData<T>(T[] data) where T : struct
		=> WriteBuffer<T>(data, BufferUsageHint.StaticDraw);
}