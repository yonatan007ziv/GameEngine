using GraphicsEngine.Components.Interfaces.Buffers;
using OpenTK.Graphics.OpenGL4;

namespace GraphicsEngine.Components.OpenGL.Buffers;

public class OpenGLIndexBuffer : OpenGLBuffer, IIndexBuffer
{
	public OpenGLIndexBuffer()
		: base(BufferTarget.ElementArrayBuffer)
	{

	}

	public void WriteData(uint[] indexes)
		=> WriteBuffer<uint>(indexes, BufferUsageHint.StaticDraw);
}