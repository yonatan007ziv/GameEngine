using OpenGLRenderer.OpenGL;
using System.Numerics;

namespace OpenGLRenderer.Models;

internal readonly struct ModelData
{
	public readonly VertexArray VertexArray { get; }
	public readonly VertexBuffer VertexBuffer { get; }
	public readonly IndexBuffer IndexBuffer { get; }

	public ModelData(VertexBuffer vertexBuffer, IndexBuffer indexBuffer)
	{
		VertexArray = new VertexArray(vertexBuffer, indexBuffer);
		VertexBuffer = vertexBuffer;
		IndexBuffer = indexBuffer;
	}
}