using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.Shared.Data;
using Silk.NET.OpenGL;
using System.Runtime.InteropServices;

namespace GraphicsEngine.Components.RendererSpecific.SilkOpenGL.Buffers;

internal class SilkOpenGLVertexArray : IVertexArray, IDisposable
{
	private readonly GL glContext;

	public uint Id { get; private set; }
	public VertexAttribPointerType Type { get; private set; }

	public SilkOpenGLVertexArray(IVertexBuffer vb, IIndexBuffer ib, AttributeLayout[] arrtibutesLayout, GL glContext)
	{
		this.glContext = glContext;
		Id = glContext.GenVertexArray();
		Type = VertexAttribPointerType.Float;

		Link(vb, ib, arrtibutesLayout);
	}

	public void Bind()
	{
		glContext.BindVertexArray(Id);
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
				glContext.VertexAttribPointer(i, arrayLayout[i].Size, SystemToSilkOpenGLType(arrayLayout[i].Type), false, (uint)totalStride.ToInt32(), offset.ToPointer());
			}

			glContext.EnableVertexAttribArray(i);

			offset += Marshal.SizeOf(arrayLayout[i].Type) * arrayLayout[i].Size;
		}

		Unbind();
	}

	private VertexAttribPointerType SystemToSilkOpenGLType(Type type)
	{
		if (type == typeof(sbyte))
			return VertexAttribPointerType.Byte;
		else if (type == typeof(byte))
			return VertexAttribPointerType.UnsignedByte;
		else if (type == typeof(short))
			return VertexAttribPointerType.Short;
		else if (type == typeof(ushort))
			return VertexAttribPointerType.UnsignedShort;
		else if (type == typeof(int))
			return VertexAttribPointerType.Int;
		else if (type == typeof(uint))
			return VertexAttribPointerType.UnsignedInt;
		else if (type == typeof(float))
			return VertexAttribPointerType.Float;
		else if (type == typeof(double))
			return VertexAttribPointerType.Double;
		else if (type == typeof(Half))
			return VertexAttribPointerType.HalfFloat;
		throw new ArgumentException("Unsupported Type");
	}

	public void Dispose()
	{
		Unbind();
		glContext.DeleteVertexArray(Id);
	}
}