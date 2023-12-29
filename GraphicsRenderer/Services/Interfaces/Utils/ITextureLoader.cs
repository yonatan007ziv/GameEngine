using GraphicsRenderer.Components.Shared;

namespace GraphicsRenderer.Services.Interfaces.Utils;

internal interface ITextureLoader
{
	TextureSource LoadTexture(string path);
}