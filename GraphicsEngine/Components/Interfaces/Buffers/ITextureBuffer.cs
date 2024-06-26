﻿using GraphicsEngine.Components.Shared;

namespace GraphicsEngine.Components.Interfaces.Buffers;

internal interface ITextureBuffer : IBuffer
{
	void Bind();
	void Unbind();
	void WriteData(TextureSource textureSrc);
	void Tile(bool tile);
}