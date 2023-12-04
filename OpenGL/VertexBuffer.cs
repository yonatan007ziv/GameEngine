using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace OpenGLRenderer.OpenGL;

internal class VertexBuffer : IDisposable
{
	public int Id { get; private set; }
	public BufferTarget TargetBuffer { get; private set; }

	public VertexBuffer(BufferTarget targetBuffer = BufferTarget.ArrayBuffer)
	{
		Id = GL.GenBuffer();
		TargetBuffer = targetBuffer;
	}

	public void WriteBuffer<T>(T[] data, BufferUsageHint usage) where T : struct
	{
		Bind();
		GL.BufferData(TargetBuffer, data.Length * Marshal.SizeOf<T>(), data, usage);
	}

	public void Bind()
	{
		GL.BindBuffer(TargetBuffer, Id);
	}

	public void Unbind()
	{
		GL.BindBuffer(TargetBuffer, 0);
	}

	public void Delete()
		=> Dispose();

	public void Dispose()
	{
		Unbind();
		GL.DeleteBuffer(Id);
	}
}