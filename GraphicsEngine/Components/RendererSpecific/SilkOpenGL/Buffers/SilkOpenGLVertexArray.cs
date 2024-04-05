using GraphicsEngine.Components.Extensions;
using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.Shared.Data;
using Silk.NET.OpenGL;
using System.Runtime.InteropServices;

namespace GraphicsEngine.Components.RendererSpecific.SilkOpenGL.Buffers;

internal class SilkOpenGLVertexArray : IVertexArray
{
	private readonly GL glContext;

	public int Id => (int)_id;

	protected uint _id { get; private set; }
	public VertexAttribPointerType Type { get; private set; }

	public SilkOpenGLVertexArray(IVertexBuffer vb, IIndexBuffer ib, AttributeLayout[] arrtibutesLayout, GL glContext)
	{
		this.glContext = glContext;
		_id = glContext.GenVertexArray();
		Type = VertexAttribPointerType.Float;

		Link(vb, ib, arrtibutesLayout);
	}

	public void Bind()
	{
		glContext.BindVertexArray(_id);
	}

	public void Unbind()
	{
		glContext.BindVertexArray(0);
	}

	private void Link(IVertexBuffer vb, IIndexBuffer ib, AttributeLayout[] arrayLayout)
	{
		Bind();

		ib.Bind();
		vb.Bind();

		// Calculate Stride Between Attributes
		IntPtr totalStride = IntPtr.Zero;
		foreach (AttributeLayout layout in arrayLayout)
			totalStride += Marshal.SizeOf(layout.Type) * layout.Size;

		IntPtr offset = IntPtr.Zero;
		for (uint i = 0; i < arrayLayout.Length; i++)
		{
			unsafe
			{
				glContext.VertexAttribPointer(i, arrayLayout[i].Size, arrayLayout[i].Type.ToSilkOpenGLType(), false, (uint)totalStride.ToInt32(), offset.ToPointer());
			}

			glContext.EnableVertexAttribArray(i);

			offset += Marshal.SizeOf(arrayLayout[i].Type) * arrayLayout[i].Size;
		}

		Unbind();
	}

	~SilkOpenGLVertexArray()
	{
		Services.Implementations.Shared.GraphicsEngine.EngineContext.FinalizedVertexArrayBuffers.Enqueue(Id);
	}
}