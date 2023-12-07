using OpenGLRenderer.OpenGL;

namespace OpenGLRenderer.Models;

internal readonly struct ModelData : IDisposable
{
	// for physics collision calculations maybe add a list of vector3 representing vertices

	public readonly VertexArray VertexArray { get; }
	public readonly VertexBuffer VertexBuffer { get; }
	public readonly IndexBuffer IndexBuffer { get; }
	public readonly int IndicesCount { get; }

	public ModelData(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, int indicesCount)
	{
		VertexArray = new VertexArray(vertexBuffer, indexBuffer);
		VertexBuffer = vertexBuffer;
		IndexBuffer = indexBuffer;
		IndicesCount = indicesCount;
	}

	public void Dispose()
	{
		VertexArray.Delete();
		VertexBuffer.Delete();
		IndexBuffer.Delete();
	}
}