using GraphicsRenderer.Components.Interfaces.Buffers;
using GraphicsRenderer.Components.Shared.Data;

namespace GraphicsRenderer.Services.Interfaces.Utils;

public interface IBufferGenerator
{
	IVertexArray GenerateVertexArray(IVertexBuffer vertexBuffer, IIndexBuffer indexBuffer, AttributeLayout[] attributesLayouts);
	IVertexBuffer GenerateVertexBuffer();
	IIndexBuffer GenerateIndexBuffer();
	ITextureBuffer GenerateTextureBuffer();
}