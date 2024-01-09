using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.OpenGL.Buffers;
using GraphicsEngine.Components.Shared.Data;
using GraphicsEngine.Components.Shared.Exceptions;
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
		catch { logger.LogCritical("Error Generating OpenGL Buffer"); throw new BufferGeneratorException(); }

	}

	public ITextureBuffer GenerateTextureBuffer()
	{
		try { return new OpenGLTextureBuffer(); }
		catch { logger.LogCritical("Error Generating OpenGL Buffe!"); throw new BufferGeneratorException(); }
	}

	public IVertexArray GenerateVertexArray(IVertexBuffer vertexBuffer, IIndexBuffer indexBuffer, AttributeLayout[] arrtibutesLayout)
	{
		try { return new OpenGLVertexArray(vertexBuffer, indexBuffer, arrtibutesLayout); }
		catch { logger.LogCritical("Error Generating OpenGL Buffer"); throw new BufferGeneratorException(); }
	}

	public IVertexBuffer GenerateVertexBuffer()
	{
		try { return new OpenGLVertexBuffer(); }
		catch { logger.LogCritical("Error Generating OpenGL Buffer"); throw new BufferGeneratorException(); }
	}
}