﻿using GraphicsEngine.Components.Interfaces.Buffers;

namespace GraphicsEngine.Components.Shared;

public class Texture
{
	public string TextureName { get; }

	private readonly ITextureBuffer textureBuffer;

	public Texture(ITextureBuffer textureBuffer, string textureName)
	{
		this.textureBuffer = textureBuffer;
		TextureName = textureName;
	}

	public void Bind() => textureBuffer.Bind();
	public void Unbind() => textureBuffer.Unbind();
}