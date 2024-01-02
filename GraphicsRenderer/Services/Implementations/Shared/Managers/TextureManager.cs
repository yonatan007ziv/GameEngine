using GraphicsRenderer.Components.Interfaces.Buffers;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;

namespace GraphicsRenderer.Services.Implementations.Shared.Managers;

internal class TextureManager : ITextureManager
{
	private readonly IFactory<string, ITextureBuffer> textureFactory;
	private readonly Dictionary<string, ITextureBuffer> textures = new Dictionary<string, ITextureBuffer>();

	public TextureManager(IFactory<string, ITextureBuffer> textureFactory)
	{
		this.textureFactory = textureFactory;
	}


	public bool GetTexture(string textureName, out ITextureBuffer texture)
	{
		if (textures.ContainsKey(textureName))
		{
			texture = textures[textureName];
			return true;
		}
		else if (textureFactory.Create(textureName, out texture))
		{
			textures.Add(textureName, texture);
			return true;
		}

		texture = default!;
		return false;
	}
}