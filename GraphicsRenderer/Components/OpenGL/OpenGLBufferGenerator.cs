using GraphicsRenderer.Components.Interfaces.Buffers;
using GraphicsRenderer.Components.OpenGL.Buffers;
using GraphicsRenderer.Components.Shared.Data;
using GraphicsRenderer.Services.Interfaces.Utils;

namespace GraphicsRenderer.Components.OpenGL;

internal class OpenGLBufferGenerator : IBufferGenerator
{
	public IIndexBuffer GenerateIndexBuffer()
		=> new OpenGLIndexBuffer();

	public ITextureBuffer GenerateTextureBuffer()
		=> new OpenGLTextureBuffer();

	public IVertexArray GenerateVertexArray(IVertexBuffer vertexBuffer, IIndexBuffer indexBuffer, AttributeLayout[] arrtibutesLayout)
		=> new OpenGLVertexArray(vertexBuffer, indexBuffer, arrtibutesLayout);

	public IVertexBuffer GenerateVertexBuffer()
		=> new OpenGLVertexBuffer();
}