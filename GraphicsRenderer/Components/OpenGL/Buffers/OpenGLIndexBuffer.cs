using GraphicsRenderer.Components.Interfaces.Buffers;
using OpenTK.Graphics.OpenGL4;

namespace GraphicsRenderer.Components.OpenGL.Buffers;

public class OpenGLIndexBuffer : OpenGLBuffer, IIndexBuffer
{
	public OpenGLIndexBuffer()
		: base(BufferTarget.ElementArrayBuffer)
	{

	}

	public void WriteData(uint[] indexes)
		=> WriteBuffer<uint>(indexes, BufferUsageHint.StaticDraw);
}