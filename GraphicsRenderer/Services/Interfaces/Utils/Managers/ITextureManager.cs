using GraphicsRenderer.Components.Interfaces.Buffers;

namespace GraphicsRenderer.Services.Interfaces.Utils.Managers;

public interface ITextureManager
{
	bool GetTexture(string textureName, out ITextureBuffer result);
}