using GraphicsRenderer.Components.Interfaces.Buffers;

namespace GraphicsRenderer.Components.Shared.Data;

public readonly struct ModelData
{
	public IVertexArray VertexArray { get; }
	public BoxData BoundingBox { get; }
	public int IndicesCount { get; }

	public ModelData(IVertexArray vertexArray, BoxData boundingBox, int indicesCount)
	{
		VertexArray = vertexArray;
		BoundingBox = boundingBox;
		IndicesCount = indicesCount;
	}
}