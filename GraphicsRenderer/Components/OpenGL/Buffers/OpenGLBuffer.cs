using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace GraphicsRenderer.Components.OpenGL.Buffers;

internal abstract class OpenGLBuffer : IDisposable
{
	protected int Id { get; private set; }
	protected BufferTarget Target { get; private set; }

	public OpenGLBuffer(BufferTarget target)
	{
		Id = GL.GenBuffer();
		Target = target;
	}

	protected void WriteBuffer<T>(T[] data, BufferUsageHint usage) where T : struct
	{
		Bind();
		GL.BufferData(Target, data.Length * Marshal.SizeOf<T>(), data, usage);
	}

	public virtual void Bind()
	{
		GL.BindBuffer(Target, Id);
	}

	public virtual void Unbind()
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