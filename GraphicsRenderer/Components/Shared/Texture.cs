using GraphicsRenderer.Components.Interfaces.Buffers;

namespace GraphicsRenderer.Components.Shared;

public class Texture
{
	private readonly ITextureBuffer textureBuffer;

	public Texture(ITextureBuffer textureBuffer)
	{
		this.textureBuffer = textureBuffer;
	}

	public void Bind() => textureBuffer.Bind();
	public void Unbind() => textureBuffer.Unbind();
}