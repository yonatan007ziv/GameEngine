using GraphicsEngine.Components.Shared;

namespace GraphicsEngine.Services.Interfaces.Utils.Managers;

internal interface ITextureManager
{
    bool GetTexture(string textureName, out Texture result);
}