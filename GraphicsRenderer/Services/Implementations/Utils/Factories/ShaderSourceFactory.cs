using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;
using GraphicsRenderer.Services.Interfaces.Utils;

namespace GraphicsRenderer.Services.Implementations.Utils.Factories;

internal class ShaderSourceFactory : IFactory<string, ShaderSource>
{
	private readonly IResourceManager resourceManager;

	public ShaderSourceFactory(IResourceManager resourceManager)
	{
		this.resourceManager = resourceManager;
	}

	public ShaderSource Create(string shaderName)
	{
		return new ShaderSource(resourceManager.LoadResourceString(shaderName));
	}
}