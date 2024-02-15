using GraphicsEngine.Components.Shared;

namespace GraphicsEngine.Components.Interfaces.Buffers;

internal interface ITextureBuffer
{
	void Bind();
	void Unbind();
	void WriteData(TextureSource textureSrc);
}