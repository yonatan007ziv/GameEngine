using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;
using StbImageSharp;

namespace GraphicsRenderer.Services.Implementations.Shared;

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