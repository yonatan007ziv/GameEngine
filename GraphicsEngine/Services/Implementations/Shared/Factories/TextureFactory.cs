using GameEngine.Core.SharedServices.Interfaces;
using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Services.Interfaces.Utils;

namespace GraphicsEngine.Services.Implementations.Shared.Factories;

public class TextureFactory : IFactory<string, ITextureBuffer>
{
	private readonly ITextureLoader textureLoader;
	private readonly IBufferGenerator bufferGenerator;

	public TextureFactory(ITextureLoader textureLoader, IBufferGenerator bufferGenerator)
	{
		this.textureLoader = textureLoader;
		this.bufferGenerator = bufferGenerator;
	}

	public bool Create(string textureName, out ITextureBuffer texture)
	{
		if (!textureLoader.LoadTexture(textureName, out TextureSource textureSource))
		{
			texture = default!;
			return false;
		}

		texture = bufferGenerator.GenerateTextureBuffer();
		texture.WriteData(textureSource);
		return true;
	}
}