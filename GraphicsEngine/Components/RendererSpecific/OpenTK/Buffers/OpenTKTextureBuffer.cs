using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.Shared;
using OpenTK.Graphics.OpenGL4;

namespace GraphicsEngine.Components.RendererSpecific.OpenTK.Buffers;

internal class OpenTKTextureBuffer : OpenTKBuffer, ITextureBuffer
{
    private int Id { get; }
    private TextureTarget Target { get; }

    public OpenTKTextureBuffer()
        : base(BufferTarget.TextureBuffer)
    {
        Id = GL.GenTexture();
        Target = TextureTarget.Texture2D;
    }

    public override void Bind()
    {
        GL.BindTexture(Target, Id);
    }

    public override void Unbind()
    {
        GL.BindTexture(Target, 0);
    }

    public void WriteData(TextureSource textureSrc)
    {
        Bind();

        GL.TexParameterI(Target, TextureParameterName.TextureMinFilter, new int[] { (int)TextureMinFilter.Nearest });
        GL.TexParameterI(Target, TextureParameterName.TextureMagFilter, new int[] { (int)TextureMagFilter.Nearest });

        GL.TexImage2D(Target, 0, PixelInternalFormat.Rgba, textureSrc.Width, textureSrc.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, textureSrc.Source);

        Unbind();
    }

    public void Tile(bool tile)
    {
        TextureWrapMode wrapMode = TextureWrapMode.ClampToBorder;
        if (tile)
            wrapMode = TextureWrapMode.Repeat;

        Bind();
        GL.TexParameterI(Target, TextureParameterName.TextureWrapS, new int[] { (int)wrapMode });
        GL.TexParameterI(Target, TextureParameterName.TextureWrapT, new int[] { (int)wrapMode });
        Unbind();
    }
}
