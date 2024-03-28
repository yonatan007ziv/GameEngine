using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.Shared.Data;
using GraphicsEngine.Services.Interfaces;
using GraphicsEngine.Services.Interfaces.Utils;

namespace GraphicsEngine.Services.Implementations.Shared.Factories;

internal class BufferFactory : IBufferFactory
{
    private readonly IBufferSpecificGenerator bufferGenerator;

    public BufferFactory(IBufferSpecificGenerator bufferGenerator)
    {
        this.bufferGenerator = bufferGenerator;
    }

    public IIndexBuffer GenerateIndexBuffer()
        => bufferGenerator.GenerateIndexBuffer();

    public ITextureBuffer GenerateTextureBuffer()
        => bufferGenerator.GenerateTextureBuffer();

    public IVertexArray GenerateVertexArray(IVertexBuffer vertexBuffer, IIndexBuffer indexBuffer, AttributeLayout[] attributesLayouts)
        => bufferGenerator.GenerateVertexArray(vertexBuffer, indexBuffer, attributesLayouts);

    public IVertexBuffer GenerateVertexBuffer()
        => bufferGenerator.GenerateVertexBuffer();
}