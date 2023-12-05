using OpenTK.Graphics.OpenGL4;

namespace OpenGLRenderer.OpenGL;

internal class VertexArray : IDisposable
{
	public int Id { get; private set; }
	public VertexAttribPointerType Type { get; private set; }

	public VertexArray(VertexBuffer vb, IndexBuffer ib)
	{
		Id = GL.GenVertexArray();
		Type = VertexAttribPointerType.Float;

		LinkVbIb(vb, ib, new VertexArrayLayout());
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

	private void LinkVbIb(VertexBuffer vb, IndexBuffer ib, VertexArrayLayout layout)
	{
		Bind();

		vb.Bind();
		ib.Bind();

		GL.VertexAttribPointer(0, 3, Type, false, 12, 0);
		GL.EnableVertexAttribArray(0);

		Unbind();
	}
}

internal struct VertexArrayLayout
{
	public int ElementsPerVertex;
	public int Stride;
	public int Offset;
}