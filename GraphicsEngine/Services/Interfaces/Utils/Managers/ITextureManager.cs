using GraphicsEngine.Components.Shared;

namespace GraphicsEngine.Services.Interfaces.Utils.Managers;

public interface ITextureManager
{
	bool GetTexture(string textureName, out Texture result);
}