using GraphicsEngine.Components.Interfaces.Buffers;

namespace GraphicsEngine.Services.Interfaces.Utils.Managers;

public interface ITextureManager
{
	bool GetTexture(string textureName, out ITextureBuffer result);
}