using GameEngine.Core.SharedServices.Interfaces;
using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Services.Interfaces;
using GraphicsEngine.Services.Interfaces.Utils;

namespace GraphicsEngine.Services.Implementations.Shared.Factories;

internal class TextureFactory : IFactory<string, ITextureBuffer>
{
    private readonly ITextureLoader textureLoader;
    private readonly IBufferFactory bufferFactory;

    public TextureFactory(ITextureLoader textureLoader, IBufferFactory bufferFactory)
    {
        this.textureLoader = textureLoader;
        this.bufferFactory = bufferFactory;
    }

    public bool Create(string textureName, out ITextureBuffer texture)
    {
        if (!textureLoader.LoadTexture(textureName, out TextureSource textureSource))
        {
            texture = default!;
            return false;
        }

        texture = bufferFactory.GenerateTextureBuffer();
        texture.WriteData(textureSource);
        return true;
    }
}