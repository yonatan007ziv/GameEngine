using OpenGLRenderer.OpenGL;

namespace OpenGLRenderer.Services.Interfaces.Utils;

internal interface ITextureLoader
{
    Texture LoadTexture(string path);
}