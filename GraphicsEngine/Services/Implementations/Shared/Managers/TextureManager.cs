using GameEngine.Core.SharedServices.Interfaces;
using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Services.Interfaces.Utils.Managers;

namespace GraphicsEngine.Services.Implementations.Shared.Managers;

internal class TextureManager : ITextureManager
{
    private readonly IFactory<string, ITextureBuffer> textureFactory;
    private readonly Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

    public TextureManager(IFactory<string, ITextureBuffer> textureFactory)
    {
        this.textureFactory = textureFactory;
    }

    public bool GetTexture(string textureName, out Texture texture)
    {
        if (textures.ContainsKey(textureName))
        {
            texture = textures[textureName];
            return true;
        }
        else if (textureFactory.Create(textureName, out ITextureBuffer textureBuffer))
        {
            texture = new Texture(textureBuffer, textureName);
            textures.Add(textureName, texture);
            return true;
        }

        texture = default!;
        return false;
    }
}