using OpenGLRenderer.OpenGL;

namespace OpenGLRenderer.Models;

internal readonly struct ModelData : IDisposable
{
	// for physics collision calculations maybe add a list of vector3 representing vertices

	public readonly VertexArray VertexArray { get; }
	public readonly VertexBuffer VertexBuffer { get; }
	public readonly IndexBuffer IndexBuffer { get; }
	public readonly TextureBuffer TextureBuffer { get; }
	public readonly int IndicesCount { get; }

	public ModelData(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, TextureBuffer textureBuffer, int indicesCount)
	{
		VertexArray = new VertexArray(vertexBuffer, indexBuffer, textureBuffer);
		VertexBuffer = vertexBuffer;
		IndexBuffer = indexBuffer;
		TextureBuffer = textureBuffer;
		IndicesCount = indicesCount;
	}

	public void Dispose()
	{
		VertexArray.Delete();
		VertexBuffer.Delete();
		IndexBuffer.Delete();
	}
}