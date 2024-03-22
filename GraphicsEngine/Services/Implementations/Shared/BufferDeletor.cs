using GraphicsEngine.Services.Interfaces;

namespace GraphicsEngine.Services.Implementations.Shared;

internal class BufferDeletor : IBufferDeletor
{
	private readonly IBufferSpecificDeletor bufferSpecificDeletor;

	public BufferDeletor(IBufferSpecificDeletor bufferSpecificDeletor)
	{
		this.bufferSpecificDeletor = bufferSpecificDeletor;
	}

	public void DeleteBuffer(int id)
	{
		bufferSpecificDeletor.DeleteBuffer(id);
	}

	public void DeleteTextureBuffer(int id)
	{
		bufferSpecificDeletor.DeleteTextureBuffer(id);
	}

	public void DeleteVertexArrayBuffer(int id)
	{
		bufferSpecificDeletor.DeleteVertexArrayBuffer(id);
	}
}