using GraphicsEngine.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphicsEngine.Services.Implementations.Shared;

internal class BufferDeletor : IBufferDeletor
{
	private readonly ILogger logger;
	private readonly IBufferSpecificDeletor bufferSpecificDeletor;

	public BufferDeletor(ILogger logger, IBufferSpecificDeletor bufferSpecificDeletor)
    {
		this.logger = logger;
		this.bufferSpecificDeletor = bufferSpecificDeletor;
	}

    public void DeleteBuffer(int id)
	{
		bufferSpecificDeletor.DeleteBuffer(id);
		logger.LogInformation("Deleted buffer");
	}

	public void DeleteTextureBuffer(int id)
	{
		bufferSpecificDeletor.DeleteTextureBuffer(id);
		logger.LogInformation("Deleted texture buffer");
	}

	public void DeleteVertexArrayBuffer(int id)
	{
		bufferSpecificDeletor.DeleteVertexArrayBuffer(id);
		logger.LogInformation("Deleted vertex array buffer");
	}
}