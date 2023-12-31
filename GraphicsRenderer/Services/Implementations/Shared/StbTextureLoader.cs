using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;
using StbImageSharp;

namespace GraphicsRenderer.Services.Implementations.Shared;

internal class StbTextureLoader : ITextureLoader
{
	private readonly IResourceManager resourceManager;

	public StbTextureLoader(IResourceManager resourceManager)
	{
		this.resourceManager = resourceManager;
	}

	public TextureSource LoadTexture(string file)
	{
		FileStream stream = resourceManager.LoadResourceFileStream(file);
		ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
		return new TextureSource(image.Data, image.Width, image.Height);
	}
}