using GameEngine.Core.SharedServices.Interfaces.Utils.Managers;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Services.Interfaces.Utils;
using StbImageSharp;

namespace GraphicsEngine.Services.Implementations.Shared;

public class StbTextureLoader : ITextureLoader
{
	private readonly IResourceManager resourceManager;

	public StbTextureLoader(IResourceManager resourceManager)
	{
		this.resourceManager = resourceManager;
	}

	public bool LoadTexture(string textureName, out TextureSource textureSource)
	{
		if (resourceManager.LoadResourceFileStream(textureName, out FileStream stream))
		{
			ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
			textureSource = new TextureSource(image.Data, image.Width, image.Height);
			return true;
		}

		textureSource = default!;
		return false;
	}
}