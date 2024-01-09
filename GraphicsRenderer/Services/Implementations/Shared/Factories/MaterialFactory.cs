using GameEngine.Core.SharedServices.Interfaces;
using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Components.Interfaces.Buffers;
using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;
using Microsoft.Extensions.Logging;

namespace GraphicsRenderer.Services.Implementations.Shared.Factories;

public class MaterialFactory : IFactory<string, Material>
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

	public bool Create(string shaderName, out Material material)
	{
		bool failed = false;
		if (!shaderManager.GetShader(shaderName, out IShaderProgram shader))
		{
			logger.LogError($"Shader \"{shaderName}\" not Found");
			failed = true;
		}

		if (!textureManager.GetTexture("MissingTexture.png", out ITextureBuffer texture))
		{
			logger.LogError($"Texture \"MissingTexture.png\" not Found");
			failed = true;
		}

		if (failed)
		{
			material = default!;
			return false;
		}

		material = new Material(new Shader(shader), new Texture(texture));
		return true;
	}
}