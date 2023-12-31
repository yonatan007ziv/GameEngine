using GraphicsRenderer.Components.Interfaces.Buffers;
using GraphicsRenderer.Components.Shared.Data;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace GraphicsRenderer.Components.OpenGL.Buffers;

internal class OpenGLVertexArray : IVertexArray, IDisposable
{
	public int Id { get; private set; }
	public VertexAttribPointerType Type { get; private set; }

	public OpenGLVertexArray(IVertexBuffer vb, IIndexBuffer ib, AttributeLayout[] arrtibutesLayout)
	{
		Id = GL.GenVertexArray();
		Type = VertexAttribPointerType.Float;

		Link(vb, ib, arrtibutesLayout);
	}

	public void Bind()
	{
		GL.BindVertexArray(Id);
	}

	public void Unbind()
	{
		GL.BindVertexArray(0);
	}

	public void Dispose()
	{
		Unbind();
		GL.DeleteVertexArray(Id);
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
		for (int i = 0; i < arrayLayout.Length; i++)
		{
			GL.VertexAttribPointer(i, arrayLayout[i].Size, SystemToOpenTKType(arrayLayout[i].Type), false, totalStride.ToInt32(), offset);
			GL.EnableVertexAttribArray(i);

			offset += Marshal.SizeOf(arrayLayout[i].Type) * arrayLayout[i].Size;
		}

		Unbind();
	}

	private VertexAttribPointerType SystemToOpenTKType(Type type)
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
		throw new ArgumentException("Unsupported type for VertexAttribPointer");
	}
}