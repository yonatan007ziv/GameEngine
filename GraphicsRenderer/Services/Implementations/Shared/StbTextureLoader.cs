using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;
using StbiSharp;

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
			//Image image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
			textureSource =default!;//= new TextureSource(image.Data, image.Width, image.Height);
			return true;
		}

		textureSource = default!;
		return false;
	}
}