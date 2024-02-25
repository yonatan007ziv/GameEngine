using GraphicsEngine.Components.Extensions;
using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.Shared.Data;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace GraphicsEngine.Components.RendererSpecific.OpenTK.Buffers;

internal class OpenTKVertexArray : IVertexArray, IDisposable
{
	public int Id { get; private set; }
	public VertexAttribPointerType Type { get; private set; }

	public OpenTKVertexArray(IVertexBuffer vb, IIndexBuffer ib, AttributeLayout[] arrtibutesLayout)
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
			GL.VertexAttribPointer(i, arrayLayout[i].Size, arrayLayout[i].Type.ToOpenTKType(), false, totalStride.ToInt32(), offset);
			GL.EnableVertexAttribArray(i);

			offset += Marshal.SizeOf(arrayLayout[i].Type) * arrayLayout[i].Size;
		}

		Unbind();
	}
}