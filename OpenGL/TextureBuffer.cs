using OpenTK.Graphics.OpenGL4;

namespace OpenGLRenderer.OpenGL;

internal class TextureBuffer
{
	private int Id { get; }
	private TextureTarget Target { get; }

	public TextureBuffer()
	{
		Id = GL.GenTexture();
		Target = TextureTarget.Texture2D;
	}

	public void Bind()
	{
		GL.ActiveTexture(TextureUnit.Texture0);
		GL.BindTexture(Target, Id);
	}

	public void Unbind()
	{
		GL.BindTexture(Target, 0);
	}

	public void WriteBuffer(TextureSource textureSrc)
	{
		Bind();

		GL.TextureParameterI(Id, TextureParameterName.TextureMinFilter, new int[] { (int)TextureMinFilter.Linear });
		GL.TextureParameterI(Id, TextureParameterName.TextureMagFilter, new int[] { (int)TextureMagFilter.Linear });

		GL.TexImage2D(Target, 0, PixelInternalFormat.Rgba, textureSrc.Width, textureSrc.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, textureSrc.Source);
	}

	public void Delete()
		=> Dispose();

	public void Dispose()
	{
		Unbind();
		GL.DeleteBuffer(Id);
	}
}