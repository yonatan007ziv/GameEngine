using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.OpenGL.Buffers;
using GraphicsEngine.Components.Shared.Data;
using GraphicsEngine.Services.Interfaces.Utils;
using Microsoft.Extensions.Logging;

namespace GraphicsEngine.Components.OpenGL;

public class OpenGLBufferGenerator : IBufferGenerator
{
	private readonly ILogger logger;

	public OpenGLBufferGenerator(ILogger logger)
	{
		this.logger = logger;
	}

	public IIndexBuffer GenerateIndexBuffer()
	{
		try { return new OpenGLIndexBuffer(); }
		catch (Exception ex) { logger.LogCritical("Error generating OpenGL index buffer: {ex}", ex.ToString()); throw; }

	}

	public ITextureBuffer GenerateTextureBuffer()
	{
		try { return new OpenGLTextureBuffer(); }
		catch (Exception ex) { logger.LogCritical("Error generating OpenGL texture buffer: {ex}", ex.ToString()); throw; }
	}

	public IVertexArray GenerateVertexArray(IVertexBuffer vertexBuffer, IIndexBuffer indexBuffer, AttributeLayout[] arrtibutesLayout)
	{
		try { return new OpenGLVertexArray(vertexBuffer, indexBuffer, arrtibutesLayout); }
		catch (Exception ex) { logger.LogCritical("Error generating OpenGL vertex array: {ex}", ex.ToString()); throw; }
	}

	public IVertexBuffer GenerateVertexBuffer()
	{
		try { return new OpenGLVertexBuffer(); }
		catch (Exception ex) { logger.LogCritical("Error generating OpenGL vertex buffer: {ex}", ex.ToString()); throw; }
	}
}