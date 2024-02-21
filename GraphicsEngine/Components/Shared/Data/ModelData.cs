using GraphicsEngine.Components.Interfaces.Buffers;

namespace GraphicsEngine.Components.Shared.Data;

internal readonly struct ModelData
{
	public IVertexArray VertexArray { get; }
	public uint IndicesCount { get; }

	public ModelData(IVertexArray vertexArray, uint indicesCount)
	{
		VertexArray = vertexArray;
		IndicesCount = indicesCount;
	}
}