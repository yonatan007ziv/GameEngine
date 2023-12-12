using OpenGLRenderer.OpenGL;
using OpenGLRenderer.Services.Interfaces.Utils.Factories;

namespace OpenGLRenderer.Services.Implementations.Utils.Factories;

internal class ShaderProgramFactory : IFactory<string, string, ShaderProgram>
{
	private readonly IFactory<string, ShaderSource> shaderSourceFactory;

	public ShaderProgramFactory(IFactory<string, ShaderSource> shaderSourceFactory)
	{
		this.shaderSourceFactory = shaderSourceFactory;
	}

	public ShaderProgram Create(string vertexName, string fragmentName)
	{
		ShaderSource s1 = shaderSourceFactory.Create(vertexName);
		ShaderSource s2 = shaderSourceFactory.Create(fragmentName);

		ShaderProgram shaderProgram = new ShaderProgram(s1, s2);
		return shaderProgram;
	}
}