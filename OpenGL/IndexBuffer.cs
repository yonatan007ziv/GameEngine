using OpenTK.Graphics.OpenGL4;

namespace OpenGLRenderer.OpenGL;

internal class IndexBuffer
{
	public int Id { get; private set; }

    public IndexBuffer()
    {
        Id = GL.GenBuffer();
    }

    public void WriteBuffer(uint[] indexes)
    {
        Bind();
        GL.BufferData(BufferTarget.ElementArrayBuffer, indexes.Length * sizeof(uint), indexes, BufferUsageHint.StaticDraw);
    }

    public void Bind()
    {
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, Id);
    }

    public void Unbind()
    {
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    }
}