using GameEngine.Core.SharedServices.Interfaces;
using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;

namespace GraphicsRenderer.Services.Implementations.Shared.Factories;

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