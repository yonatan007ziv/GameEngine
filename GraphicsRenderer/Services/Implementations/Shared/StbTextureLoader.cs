using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Services.Interfaces.Utils;
using StbImageSharp;

namespace GraphicsRenderer.Services.Implementations.Shared;

internal class StbTextureLoader : ITextureLoader
{
	public TextureSource LoadTexture(string path)
	{
		using var stream = File.OpenRead(path);
		ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
		return new TextureSource(image.Data, image.Width, image.Height);
	}
}