using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace OpenGLRenderer.OpenGL;

internal abstract class GLBuffer : IDisposable
{
	protected int Id { get; private set; }
	protected BufferTarget Target { get; private set; }

	public GLBuffer(BufferTarget target)
	{
		Id = GL.GenBuffer();
		Target = target;
	}

	protected void WriteBuffer<T>(T[] data, BufferUsageHint usage) where T : struct
	{
		Bind();
		GL.BufferData(Target, data.Length * Marshal.SizeOf<T>(), data, usage);
	}

	public void Bind()
	{
		GL.BindBuffer(Target, Id);
	}

	public void Unbind()
	{
		GL.BindBuffer(Target, 0);
	}

	public void Delete()
		=> Dispose();

	public void Dispose()
	{
		Unbind();
		GL.DeleteBuffer(Id);
	}
}