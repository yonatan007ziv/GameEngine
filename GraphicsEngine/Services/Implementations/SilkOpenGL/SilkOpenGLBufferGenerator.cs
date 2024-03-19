using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.RendererSpecific.SilkOpenGL.Buffers;
using GraphicsEngine.Components.Shared.Data;
using GraphicsEngine.Services.Interfaces.Utils;
using Microsoft.Extensions.Logging;

namespace GraphicsEngine.Services.Implementations.SilkOpenGL;

internal class SilkOpenGLBufferGenerator : IBufferSpecificGenerator
{
    private readonly ILogger logger;

    public SilkOpenGLBufferGenerator(ILogger logger)
    {
        this.logger = logger;
    }

    public IIndexBuffer GenerateIndexBuffer()
    {
        try { return new SilkOpenGLIndexBuffer(SilkOpenGLContext.Instance.silkOpenGLContext); }
        catch (Exception ex) { logger.LogCritical("Error generating SilkOpenGL index buffer: {ex}", ex.ToString()); throw; }
    }

    public ITextureBuffer GenerateTextureBuffer()
    {
        try { return new SilkOpenGLTextureBuffer(SilkOpenGLContext.Instance.silkOpenGLContext); }
        catch (Exception ex) { logger.LogCritical("Error generating SilkOpenGL index buffer: {ex}", ex.ToString()); throw; }
    }

    public IVertexArray GenerateVertexArray(IVertexBuffer vertexBuffer, IIndexBuffer indexBuffer, AttributeLayout[] attributesLayouts)
    {
        try { return new SilkOpenGLVertexArray(vertexBuffer, indexBuffer, attributesLayouts, SilkOpenGLContext.Instance.silkOpenGLContext); }
        catch (Exception ex) { logger.LogCritical("Error generating SilkOpenGL index buffer: {ex}", ex.ToString()); throw; }
    }

    public IVertexBuffer GenerateVertexBuffer()
    {
        try { return new SilkOpenGLVertexBuffer(SilkOpenGLContext.Instance.silkOpenGLContext); }
        catch (Exception ex) { logger.LogCritical("Error generating SilkOpenGL index buffer: {ex}", ex.ToString()); throw; }
    }
}