using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.Shared.Data;

namespace GraphicsEngine.Services.Interfaces.Utils;

internal interface IBufferSpecificGenerator
{
    IVertexArray GenerateVertexArray(IVertexBuffer vertexBuffer, IIndexBuffer indexBuffer, AttributeLayout[] attributesLayouts);
    IVertexBuffer GenerateVertexBuffer();
    IIndexBuffer GenerateIndexBuffer();
    ITextureBuffer GenerateTextureBuffer();
}