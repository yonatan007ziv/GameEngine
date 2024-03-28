using GraphicsEngine.Services.Interfaces;
using OpenTK.Graphics.OpenGL4;

namespace GraphicsEngine.Services.Implementations.OpenTK;

internal class OpenTKBufferDeletor : IBufferSpecificDeletor
{
    public void DeleteBuffer(int id)
        => GL.DeleteBuffer(id);

    public void DeleteTextureBuffer(int id)
        => GL.DeleteTexture(id);

    public void DeleteVertexArrayBuffer(int id)
        => GL.DeleteVertexArray(id);
}