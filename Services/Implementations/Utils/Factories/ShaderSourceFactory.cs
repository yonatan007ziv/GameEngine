using OpenGLRenderer.OpenGL;
using OpenGLRenderer.Services.Interfaces.Utils.Factories;
using OpenGLRenderer.Services.Interfaces.Utils.Managers;

namespace OpenGLRenderer.Services.Implementations.Utils.Factories;

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