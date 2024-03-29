using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.RendererSpecific.OpenTK.Buffers;
using GraphicsEngine.Components.Shared.Data;
using GraphicsEngine.Services.Interfaces.Utils;
using Microsoft.Extensions.Logging;

namespace GraphicsEngine.Services.Implementations.OpenTK;

internal class OpenTKBufferGenerator : IBufferSpecificGenerator
{
	private readonly ILogger logger;

	public OpenTKBufferGenerator(ILogger logger)
	{
		this.logger = logger;
	}

	public IIndexBuffer GenerateIndexBuffer()
	{
		try { return new OpenTKIndexBuffer(); }
		catch (Exception ex) { logger.LogCritical("Error generating OpenGL index buffer: {ex}", ex.ToString()); throw; }

	}

	public ITextureBuffer GenerateTextureBuffer()
	{
		try { return new OpenTKTextureBuffer(); }
		catch (Exception ex) { logger.LogCritical("Error generating OpenGL texture buffer: {ex}", ex.ToString()); throw; }
	}

	public IVertexArray GenerateVertexArray(IVertexBuffer vertexBuffer, IIndexBuffer indexBuffer, AttributeLayout[] arrtibutesLayout)
	{
		try { return new OpenTKVertexArray(vertexBuffer, indexBuffer, arrtibutesLayout); }
		catch (Exception ex) { logger.LogCritical("Error generating OpenGL vertex array: {ex}", ex.ToString()); throw; }
	}

	public IVertexBuffer GenerateVertexBuffer()
	{
		try { return new OpenTKVertexBuffer(); }
		catch (Exception ex) { logger.LogCritical("Error generating OpenGL vertex buffer: {ex}", ex.ToString()); throw; }
	}
}