using OpenTK.Graphics.OpenGL4;

namespace OpenGLRenderer.OpenGL;

internal class IndexBuffer : GLBuffer
{
	public IndexBuffer()
		: base(BufferTarget.ElementArrayBuffer)
	{

	}

	public void WriteBuffer(uint[] indexes)
		=> WriteBuffer<uint>(indexes, BufferUsageHint.StaticDraw);
}