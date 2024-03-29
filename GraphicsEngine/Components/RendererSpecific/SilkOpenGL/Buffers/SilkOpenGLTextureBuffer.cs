using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.Shared;
using Silk.NET.OpenGL;

namespace GraphicsEngine.Components.RendererSpecific.SilkOpenGL.Buffers;

internal class SilkOpenGLTextureBuffer : ITextureBuffer
{
	private readonly GL glContext;

	public int Id => (int)_id;

	private uint _id { get; }
	private TextureTarget Target { get; }

	public SilkOpenGLTextureBuffer(GL glContext)
	{
		this.glContext = glContext;

		_id = glContext.GenTexture();
		Target = TextureTarget.Texture2D;
	}

	public void Bind()
	{
		glContext.BindTexture(Target, _id);
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

	~SilkOpenGLTextureBuffer()
	{
		Services.Implementations.Shared.GraphicsEngine.EngineContext.FinalizedTextureBuffers.Add(Id);
	}
}