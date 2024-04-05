using GraphicsEngine.Components.Interfaces.Buffers;
using Silk.NET.OpenGL;
using System.Runtime.InteropServices;

namespace GraphicsEngine.Components.RendererSpecific.SilkOpenGL.Buffers;

internal abstract class SilkOpenGLBuffer : IBuffer
{
	private readonly GL glContext;

	public int Id => (int)_id;

	protected uint _id { get; private set; }
	protected BufferTargetARB Target { get; private set; }

	public SilkOpenGLBuffer(BufferTargetARB target, GL glContext)
	{
		this.glContext = glContext;

		_id = glContext.GenBuffer();
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
		glContext.BindBuffer(Target, _id);
	}

	public virtual void Unbind()
	{
		glContext.BindBuffer(Target, 0u);
	}

	public virtual void DeleteBuffer()
	{
		Console.WriteLine("SilkOpenGL Buffer: fix dispose cleanup");
		Unbind();
		glContext.DeleteBuffer(_id);
	}

	~SilkOpenGLBuffer()
	{
		Services.Implementations.Shared.GraphicsEngine.EngineContext.FinalizedBuffers.Enqueue(Id);
	}
}