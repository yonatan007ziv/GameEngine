using OpenGLRenderer.OpenGL;

namespace OpenGLRenderer.Services.Interfaces.Utils;

internal interface ITextureLoader
{
	TextureSource LoadTexture(string path);
}