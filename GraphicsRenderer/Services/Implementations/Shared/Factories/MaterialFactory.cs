using GraphicsRenderer.Components.Interfaces.Buffers;
using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;

namespace GraphicsRenderer.Services.Implementations.Shared.Factories;

internal class MaterialFactory : IFactory<string, string, Material>
{
	private readonly IShaderManager shaderManager;
	private readonly IFactory<string, ITextureBuffer> textureFactory;

	public MaterialFactory(IShaderManager shaderManager, IFactory<string, ITextureBuffer> textureFactory)
	{
		this.shaderManager = shaderManager;
		this.textureFactory = textureFactory;
	}

	public Material Create(string shader, string texture)
	{
		return new Material(shaderManager.GetShader(shader), textureFactory.Create(texture));
	}
}