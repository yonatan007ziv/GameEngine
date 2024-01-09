using GraphicsEngine.Components.Shared;

namespace GraphicsEngine.Services.Interfaces.Utils;

public interface ITextureLoader
{
	bool LoadTexture(string path, out TextureSource textureSource);
}