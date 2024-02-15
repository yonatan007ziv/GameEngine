using GameEngine.Core.Components;
using GraphicsEngine.Components.Interfaces.Buffers;

namespace GraphicsEngine.Components.Shared.Data;

internal readonly struct ModelData
{
	public IVertexArray VertexArray { get; }
	public BoxData BoundingBox { get; }
	public uint IndicesCount { get; }

	public ModelData(IVertexArray vertexArray, BoxData boundingBox, uint indicesCount)
	{
		VertexArray = vertexArray;
		BoundingBox = boundingBox;
		IndicesCount = indicesCount;
	}
}