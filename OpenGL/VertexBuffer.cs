using OpenTK.Graphics.OpenGL4;

namespace OpenGLRenderer.OpenGL;

internal class VertexBuffer : GLBuffer
{

	public VertexBuffer()
		: base(BufferTarget.ArrayBuffer)
	{

	}

	public void WriteBuffer<T>(T[] data) where T : struct
		=> WriteBuffer<T>(data, BufferUsageHint.StaticDraw);
}