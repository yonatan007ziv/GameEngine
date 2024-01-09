using GameEngine.Core.SharedServices.Interfaces;
using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Components.OpenGL;
using GraphicsRenderer.Components.Shared;

namespace GraphicsRenderer.Services.Implementations.Shared.Factories.Shaders;

public class ShaderProgramFactory : IFactory<string, string, IShaderProgram>
{
	private readonly IFactory<string, ShaderSource> shaderSourceFactory;

	public ShaderProgramFactory(IFactory<string, ShaderSource> shaderSourceFactory)
	{
		this.shaderSourceFactory = shaderSourceFactory;
	}

	public bool Create(string vertexName, string fragmentName, out IShaderProgram shader)
	{
		shader = default!;

		if (!shaderSourceFactory.Create(vertexName, out ShaderSource vertexSource))
			return false;
		if (!shaderSourceFactory.Create(fragmentName, out ShaderSource fragmentSource))
			return false;

		shader = new OpenGLShaderProgram(vertexSource, fragmentSource);
		return true;
	}
}