using GraphicsRenderer.Components.Interfaces.Buffers;

namespace GraphicsRenderer.Components.Shared.Data;

internal readonly struct Model3DData
{
    public IVertexArray VertexArray { get; }
    public BoxData BoundingBox { get; }
    public int IndicesCount { get; }

    public Model3DData(IVertexArray vertexArray, BoxData boundingBox, int indicesCount)
    {
        VertexArray = vertexArray;
        BoundingBox = boundingBox;
        IndicesCount = indicesCount;
    }
}