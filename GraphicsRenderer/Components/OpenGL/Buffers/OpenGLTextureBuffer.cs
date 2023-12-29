using GraphicsRenderer.Components.Interfaces.Buffers;
using GraphicsRenderer.Components.Shared;
using OpenTK.Graphics.OpenGL4;

namespace GraphicsRenderer.Components.OpenGL.Buffers;

internal class OpenGLTextureBuffer : ITextureBuffer
{
	private int Id { get; }
	private TextureTarget Target { get; }

	public OpenGLTextureBuffer()
	{
		Id = GL.GenTexture();
		Target = TextureTarget.Texture2D;
	}

	public void Bind()
	{
		GL.BindTexture(Target, Id);
	}

	public void Unbind()
	{
		GL.BindTexture(Target, 0);
	}

	public void WriteData(TextureSource textureSrc)
	{
		Bind();

		GL.TextureParameterI(Id, TextureParameterName.TextureMinFilter, new int[] { (int)TextureMinFilter.Linear });
		GL.TextureParameterI(Id, TextureParameterName.TextureMagFilter, new int[] { (int)TextureMagFilter.Linear });

		GL.TexImage2D(Target, 0, PixelInternalFormat.Rgba, textureSrc.Width, textureSrc.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, textureSrc.Source);
	}

	public void Dispose()
	{
		Unbind();
		GL.DeleteBuffer(Id);
	}
}
