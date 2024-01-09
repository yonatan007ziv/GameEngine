using GameEngine.Core.SharedServices.Interfaces;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Services.Interfaces.Utils.Managers;

namespace GraphicsEngine.Services.Implementations.Shared.Factories;

public class ShaderSourceFactory : IFactory<string, ShaderSource>
{
	private readonly IResourceManager resourceManager;

	public ShaderSourceFactory(IResourceManager resourceManager)
	{
		this.resourceManager = resourceManager;
	}

	public bool Create(string shaderName, out ShaderSource shaderSource)
	{
		if (resourceManager.LoadResourceString(shaderName, out string shaderSourceCode))
		{
			shaderSource = new ShaderSource(shaderSourceCode);
			return true;
		}

		shaderSource = default!;
		return false;
	}
}