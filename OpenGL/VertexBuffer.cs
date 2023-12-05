using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

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