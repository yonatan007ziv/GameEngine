using OpenGLRenderer.Components;
using OpenGLRenderer.OpenGL;

namespace OpenGLRenderer.Models;

internal readonly struct ModelData : IDisposable
{
	// for physics collision calculations maybe add a list of vector3 representing vertices

	public VertexArray VertexArray { get; }
	public VertexBuffer VertexBuffer { get; }
	public IndexBuffer IndexBuffer { get; }
	public TextureBuffer TextureBuffer { get; }
	public Box BoundingBox { get; }
	public int IndicesCount { get; }

	public ModelData(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, TextureBuffer textureBuffer, Box boundingBox, int indicesCount)
	{
		VertexArray = new VertexArray(vertexBuffer, indexBuffer, textureBuffer);
		VertexBuffer = vertexBuffer;
		IndexBuffer = indexBuffer;
		TextureBuffer = textureBuffer;
		BoundingBox = boundingBox;
		IndicesCount = indicesCount;
	}

	public void Dispose()
	{
		VertexArray.Delete();
		VertexBuffer.Delete();
		IndexBuffer.Delete();
	}
}