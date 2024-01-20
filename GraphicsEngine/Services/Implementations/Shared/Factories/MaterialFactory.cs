using GameEngine.Core.SharedServices.Interfaces;
using GameEngine.Core.SharedServices.Interfaces.Utils.Managers;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Services.Interfaces.Utils.Managers;
using Microsoft.Extensions.Logging;

namespace GraphicsEngine.Services.Implementations.Shared.Factories;

internal class MaterialFactory : IFactory<string, Material>
{
	private readonly ILogger logger;
	private readonly IResourceManager resourceManager;
	private readonly IShaderManager shaderManager;
	private readonly ITextureManager textureManager;

	public MaterialFactory(ILogger logger, IResourceManager resourceManager, IShaderManager shaderManager, ITextureManager textureManager)
	{
		this.logger = logger;
		this.resourceManager = resourceManager;
		this.shaderManager = shaderManager;
		this.textureManager = textureManager;
	}

	public bool Create(string materialName, out Material material)
	{
		if (!resourceManager.LoadResourceLines(materialName, out string[] mat) || mat.Length < 2)
		{
			if (materialName == "Default.mat")
			{
				logger.LogError("Failed falling back to default material", materialName);
				material = default!;
				return false;
			}

			logger.LogError("Material {material} not found, falling back to default material", materialName);
			return Create("Default.mat", out material);
		}

		(string shaderName, string textureName) = (mat[0], mat[1]);
		if (!shaderManager.GetShader(shaderName, out Shader shader))
		{
			logger.LogError("Shader {shader} not found, falling back to default shader", shaderName);
			if (!shaderManager.GetShader(shaderName, out shader))
			{
				logger.LogCritical("Failed falling back to default shader", shaderName);
				material = default!;
				return false;
			}
		}

		if (!textureManager.GetTexture(textureName, out Texture texture))
		{
			logger.LogError("Texture {texture} not found, falling back to default texture", textureName);
			if (!textureManager.GetTexture("Default.png", out texture))
			{
				logger.LogCritical("Failed falling back to default texture", textureName);
				material = default!;
				return false;
			}
		}

		material = new Material(shader, texture);
		return true;
	}
}