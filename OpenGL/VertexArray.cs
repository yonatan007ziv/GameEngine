using OpenTK.Graphics.OpenGL4;

namespace OpenGLRenderer.OpenGL;

internal class VertexArray : IDisposable
{
	public int Id { get; private set; }
	public VertexAttribPointerType Type { get; private set; }

	public VertexArray(VertexBuffer vb, IndexBuffer ib, TextureBuffer tb)
	{
		Id = GL.GenVertexArray();
		Type = VertexAttribPointerType.Float;

		Link(vb, ib, tb, new VertexArrayLayout());
	}

	public void Bind()
	{
		GL.BindVertexArray(Id);
	}

	public void Unbind()
	{
		GL.BindVertexArray(0);
	}

	public void Delete()
		=> Dispose();

	public void Dispose()
	{
		Unbind();
		GL.DeleteVertexArray(Id);
	}

	private void Link(VertexBuffer vb, IndexBuffer ib, TextureBuffer tb, VertexArrayLayout layout)
	{
		Bind();

		vb.Bind();
		ib.Bind();
		tb.Bind();

		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 5, IntPtr.Zero);
		GL.EnableVertexAttribArray(0);

		GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 5, (IntPtr)(sizeof(float) * 3));
		GL.EnableVertexAttribArray(1);

		Unbind();
	}
}

internal struct VertexArrayLayout
{
	public int ElementsPerVertex;
	public int Stride;
	public int Offset;
}