using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.Shared;
using Silk.NET.OpenGL;
using PixelFormat = Silk.NET.OpenGL.PixelFormat;
using PixelType = Silk.NET.OpenGL.PixelType;

namespace GraphicsEngine.Components.RendererSpecific.SilkOpenGL.Buffers;

internal class SilkOpenGLTextureBuffer : ITextureBuffer, IDisposable
{
	private readonly GL glContext;

	private uint Id { get; }
	private TextureTarget Target { get; }

	public SilkOpenGLTextureBuffer(GL glContext)
	{
		this.glContext = glContext;

		Id = glContext.GenTexture();
		Target = TextureTarget.Texture2D;
	}

	public void Bind()
	{
		glContext.BindTexture(Target, Id);
	}

	public void Unbind()
	{
		glContext.BindTexture(Target, 0);
	}

	public void WriteData(TextureSource textureSrc)
	{
		Bind();

		glContext.TexParameterI(Target, TextureParameterName.TextureMinFilter, new int[] { (int)TextureMinFilter.Nearest });
		glContext.TexParameterI(Target, TextureParameterName.TextureMagFilter, new int[] { (int)TextureMagFilter.Nearest });

		unsafe
		{
			fixed (byte* ptr = &textureSrc.Source[0])
				glContext.TexImage2D(Target, 0, InternalFormat.Rgba, (uint)textureSrc.Width, (uint)textureSrc.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ptr);
		}

		Unbind();
	}

	public void Tile(bool tile)
	{
		TextureWrapMode wrapMode = TextureWrapMode.ClampToBorder;
		if (tile)
			wrapMode = TextureWrapMode.Repeat;

		Bind();
		glContext.TexParameterI(Target, TextureParameterName.TextureWrapS, new int[] { (int)wrapMode });
		glContext.TexParameterI(Target, TextureParameterName.TextureWrapT, new int[] { (int)wrapMode });
		Unbind();
	}

	public void Dispose()
	{
		throw new NotImplementedException();
	}
}