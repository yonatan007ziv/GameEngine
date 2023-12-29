using GraphicsRenderer.Components.Interfaces.Buffers;

namespace GraphicsRenderer.Services.Interfaces.Utils;

internal interface IBufferGenerator
{
	IVertexArray GenerateVertexArray(IVertexBuffer vertexBuffer, IIndexBuffer indexBuffer, ITextureBuffer textureBuffer);
	IVertexBuffer GenerateVertexBuffer();
	IIndexBuffer GenerateIndexBuffer();
	ITextureBuffer GenerateTextureBuffer();
}