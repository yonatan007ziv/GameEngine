using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Components.Interfaces.Buffers;
using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;
using Microsoft.Extensions.Logging;

namespace GraphicsRenderer.Services.Implementations.Shared.Factories;

public class MaterialFactory : IFactory<string, string, Material>
{
	private readonly ILogger logger;
	private readonly IShaderManager shaderManager;
	private readonly ITextureManager textureManager;

	public MaterialFactory(ILogger logger, IShaderManager shaderManager, ITextureManager textureManager)
	{
		this.logger = logger;
		this.shaderManager = shaderManager;
		this.textureManager = textureManager;
	}

	public bool Create(string shaderName, string textureName, out Material material)
	{
		bool failed = false;
		if (!shaderManager.GetShader(shaderName, out IShaderProgram shader))
		{
			logger.LogError($"Shader \"{shaderName}\" Not Found!");
			failed = true;
		}

		if (!textureManager.GetTexture(textureName, out ITextureBuffer texture))
		{
			logger.LogError($"Texture \"{textureName}\" Not Found!");
			failed = true;
		}

		if (failed)
		{
			material = default!;
			return false;
		}

		material= new Material(shader, texture);
		return true;
	}
}