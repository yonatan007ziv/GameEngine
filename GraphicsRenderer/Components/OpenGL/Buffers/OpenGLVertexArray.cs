using GraphicsRenderer.Components.Interfaces.Buffers;
using OpenTK.Graphics.OpenGL4;

namespace GraphicsRenderer.Components.OpenGL.Buffers;

internal class OpenGLVertexArray : IVertexArray, IDisposable
{
	private readonly ITextureBuffer tb;

	public int Id { get; private set; }
	public VertexAttribPointerType Type { get; private set; }

	public OpenGLVertexArray(IVertexBuffer vb, IIndexBuffer ib, ITextureBuffer tb)
	{
		Id = GL.GenVertexArray();
		Type = VertexAttribPointerType.Float;

		this.tb = tb;

		AttributeLayout[] arr = new AttributeLayout[] { new AttributeLayout(VertexAttribPointerType.Float, 3), new AttributeLayout(VertexAttribPointerType.Float, 2) };
		Link(vb, ib, arr);
	}

	public void Bind()
	{
		tb.Bind();
		GL.BindVertexArray(Id);
	}

	public void Unbind()
	{
		tb.Unbind();
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

		vb.Bind();

		// Calculate Stride Between Attributes
		IntPtr totalStride = IntPtr.Zero;
		foreach (AttributeLayout layout in arrayLayout)
		{
			// Add Other Types Later
			if (layout.Type == VertexAttribPointerType.Float)
				totalStride += sizeof(float) * layout.Size;
		}

		IntPtr offset = IntPtr.Zero;
		for (int i = 0; i < arrayLayout.Length; i++)
		{
			GL.VertexAttribPointer(i, arrayLayout[i].Size, arrayLayout[i].Type, false, sizeof(float) * 5, offset);
			GL.EnableVertexAttribArray(i);

			if (arrayLayout[i].Type == VertexAttribPointerType.Float)
				offset += sizeof(float) * arrayLayout[i].Size;
		}

		//GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 5, IntPtr.Zero);
		//GL.EnableVertexAttribArray(0);
		//
		//GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 5, (IntPtr)(sizeof(float) * 3));
		//GL.EnableVertexAttribArray(1);

		ib.Bind();

		Unbind();
	}
}

internal struct AttributeLayout
{
	public AttributeLayout(VertexAttribPointerType type, int size)
	{
		Type = type;
		Size = size;
	}

	public VertexAttribPointerType Type;
	public int Size;
}