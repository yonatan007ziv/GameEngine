using GraphicsEngine.Components.Interfaces.Buffers;
using OpenTK.Graphics.OpenGL4;

namespace GraphicsEngine.Components.RendererSpecific.OpenTK.Buffers;

internal class OpenTKIndexBuffer : OpenTKBuffer, IIndexBuffer
{
	public OpenTKIndexBuffer()
		: base(BufferTarget.ElementArrayBuffer)
	{

	}

	public void WriteData(uint[] indexes)
		=> WriteBuffer(indexes, BufferUsageHint.StaticDraw);
}