using GraphicsRenderer.Components.Shared;

namespace GraphicsRenderer.Components.Interfaces.Buffers;

internal interface ITextureBuffer
{
	void Bind();
	void Unbind();
	void WriteData(TextureSource textureSrc);
}