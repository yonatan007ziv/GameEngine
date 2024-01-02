using GraphicsRenderer.Components.Shared;

namespace GraphicsRenderer.Services.Interfaces.Utils;

public interface ITextureLoader
{
	bool LoadTexture(string path, out TextureSource textureSource);
}