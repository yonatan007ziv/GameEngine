using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Components.Interfaces.Buffers;

namespace GraphicsRenderer.Components.Shared;

public class Material
{
	public IShaderProgram ShaderProgram { get; }
	public ITextureBuffer TextureBuffer { get; }

	public Material(IShaderProgram shaderProgram, ITextureBuffer textureBuffer)
	{
		this.ShaderProgram = shaderProgram;
		this.TextureBuffer = textureBuffer;
	}

	public void Bind()
	{
		ShaderProgram.Bind();
		TextureBuffer.Bind();
	}

	public void Unbind()
	{
		ShaderProgram.Unbind();
		TextureBuffer.Unbind();
	}
}