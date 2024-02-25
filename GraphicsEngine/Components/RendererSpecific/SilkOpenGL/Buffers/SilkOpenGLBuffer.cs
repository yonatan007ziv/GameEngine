using Silk.NET.OpenGL;
using System.Runtime.InteropServices;

namespace GraphicsEngine.Components.RendererSpecific.SilkOpenGL.Buffers;

internal abstract class SilkOpenGLBuffer : IDisposable
{
	private readonly GL glContext;

	protected uint Id { get; private set; }
	protected BufferTargetARB Target { get; private set; }

	public SilkOpenGLBuffer(BufferTargetARB target, GL glContext)
	{
		this.glContext = glContext;

		Id = glContext.GenBuffer();
		Target = target;
	}

	protected void WriteBuffer<T>(T[] data, BufferUsageARB usage) where T : struct
	{
		unsafe
		{
			fixed (void* ptr = &data[0])
			{
				Bind();
				glContext.BufferData(Target, (nuint)(data.Length * Marshal.SizeOf<T>()), ptr, usage);
			}
		}
	}

	public virtual void Bind()
	{
		glContext.BindBuffer(Target, Id);
	}

	public virtual void Unbind()
	{
		glContext.BindBuffer(Target, 0u);
	}

	#region Dispose pattern
	private bool disposedValue;
	~SilkOpenGLBuffer()
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
			// glContext.DeleteBuffer(Id);

			disposedValue = true;
		}
	}
	#endregion
}