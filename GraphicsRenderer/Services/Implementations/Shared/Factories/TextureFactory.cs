using GraphicsRenderer.Components.Interfaces.Buffers;
using GraphicsRenderer.Services.Interfaces.Utils;

namespace GraphicsRenderer.Services.Implementations.Shared.Factories;

internal class TextureFactory : IFactory<string, ITextureBuffer>
{
	private readonly ITextureLoader textureLoader;
	private readonly IBufferGenerator bufferGenerator;

	public TextureFactory(ITextureLoader textureLoader, IBufferGenerator bufferGenerator)
	{
		this.textureLoader = textureLoader;
		this.bufferGenerator = bufferGenerator;
	}

	public ITextureBuffer Create(string texture)
	{
		ITextureBuffer tb = bufferGenerator.GenerateTextureBuffer();
		tb.WriteData(textureLoader.LoadTexture(texture));
		return tb;
	}
}