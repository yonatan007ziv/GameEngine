using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace GraphicsEngine.Components.RendererSpecific.OpenTK.Buffers;

public abstract class OpenTKBuffer : IDisposable
{
	protected int Id { get; private set; }
	protected BufferTarget Target { get; private set; }

	public OpenTKBuffer(BufferTarget target)
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

	#region Dispose pattern
	private bool disposedValue;
	~OpenTKBuffer()
	{
		Dispose(disposing: false);
	}
	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{

			}

			Console.WriteLine("SilkOpenGL Buffer: fix dispose cleanup");
			// Unbind();
			// GL.DeleteBuffer(Id);

			disposedValue = true;
		}
	}
	#endregion
}