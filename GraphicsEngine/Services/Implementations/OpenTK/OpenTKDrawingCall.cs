using GraphicsEngine.Components.Shared.Data;
using GraphicsEngine.Services.Interfaces;
using OpenTK.Graphics.OpenGL4;

namespace GraphicsEngine.Services.Implementations.OpenTK;

internal class OpenTKDrawingCall : IDrawingCall
{
    public void DrawCall(ModelData modelData)
    {
        modelData.VertexArray.Bind();
        GL.DrawElements(PrimitiveType.Triangles, (int)modelData.IndicesCount, DrawElementsType.UnsignedInt, 0);
        modelData.VertexArray.Unbind();
    }
}