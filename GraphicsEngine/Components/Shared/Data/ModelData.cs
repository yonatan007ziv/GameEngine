using GameEngine.Core.Components;
using GraphicsEngine.Components.Interfaces.Buffers;

namespace GraphicsEngine.Components.Shared.Data;

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