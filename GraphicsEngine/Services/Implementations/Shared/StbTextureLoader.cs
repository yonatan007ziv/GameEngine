using GameEngine.Core.SharedServices.Interfaces.Utils.Managers;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Services.Interfaces.Utils;
using StbImageSharp;

namespace GraphicsEngine.Services.Implementations.Shared;

public class StbTextureLoader : ITextureLoader
{
    private const bool flipHorizontally = true;
    private const bool flipVertically = true;

    private readonly IResourceManager resourceManager;

    public StbTextureLoader(IResourceManager resourceManager)
    {
        this.resourceManager = resourceManager;

        if (flipVertically)
            StbImage.stbi_set_flip_vertically_on_load(1);
    }

    public bool LoadTexture(string textureName, out TextureSource textureSource)
    {
        if (resourceManager.LoadResourceFileStream(textureName, out FileStream stream))
        {
            ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

            if (flipHorizontally)
                FlipHorizontally(image);

            textureSource = new TextureSource(image.Data, image.Width, image.Height);
            return true;
        }

        textureSource = default!;
        return false;
    }

    private static void FlipHorizontally(ImageResult image)
    {
        int bytesPerPixel = 4 * sizeof(byte);
        int stride = image.Width * bytesPerPixel;
        for (int y = 0; y < image.Height; y++)
        {
            byte[] row = image.Data.Skip(y * stride).Take(stride).ToArray();
            for (int i = 0; i < row.Length; i += 4)
            {
                // Swap
                (row[i + 1], row[i + 2]) = (row[i + 2], row[i + 1]);
                (row[i + 0], row[i + 3]) = (row[i + 3], row[i + 0]);
            }

            Array.Reverse(row);
            Array.Copy(row, 0, image.Data, y * stride, stride);
        }
    }
}