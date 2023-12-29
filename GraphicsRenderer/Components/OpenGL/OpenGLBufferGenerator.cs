using GraphicsRenderer.Components.Interfaces.Buffers;
using GraphicsRenderer.Components.OpenGL.Buffers;
using GraphicsRenderer.Services.Interfaces.Utils;

namespace GraphicsRenderer.Components.OpenGL;

internal class OpenGLBufferGenerator : IBufferGenerator
{
	public IIndexBuffer GenerateIndexBuffer()
		=> new OpenGLIndexBuffer();

	public ITextureBuffer GenerateTextureBuffer()
		=> new OpenGLTextureBuffer();

	public IVertexArray GenerateVertexArray(IVertexBuffer vertexBuffer, IIndexBuffer indexBuffer, ITextureBuffer textureBuffer)
		=> new OpenGLVertexArray(vertexBuffer, indexBuffer, textureBuffer);

	public IVertexBuffer GenerateVertexBuffer()
		=> new OpenGLVertexBuffer();
}