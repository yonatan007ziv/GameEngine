using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.Shared.Data;

namespace GraphicsEngine.Services.Interfaces;

internal interface IBufferFactory
{
    IVertexArray GenerateVertexArray(IVertexBuffer vertexBuffer, IIndexBuffer indexBuffer, AttributeLayout[] attributesLayouts);
    IVertexBuffer GenerateVertexBuffer();
    IIndexBuffer GenerateIndexBuffer();
    ITextureBuffer GenerateTextureBuffer();
}