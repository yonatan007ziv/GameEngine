using OpenGLRenderer.OpenGL;
using OpenGLRenderer.Services.Interfaces.Utils;
using StbImageSharp;

namespace OpenGLRenderer.Services.Implementations.Utils;

internal class StbTextureLoader : ITextureLoader
{
	public Texture LoadTexture(string path)
	{
		using var stream = File.OpenRead(path);
		ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
		return new Texture(image.Width, image.Height, image.Data);
	}
}