using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Components.OpenGL;
using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Services.Interfaces.Utils;

namespace GraphicsRenderer.Services.Implementations.Utils.Factories.Shaders;

internal class ShaderProgramFactory : IFactory<string, string, IShaderProgram>
{
	private readonly IFactory<string, ShaderSource> shaderSourceFactory;

	public ShaderProgramFactory(IFactory<string, ShaderSource> shaderSourceFactory)
	{
		this.shaderSourceFactory = shaderSourceFactory;
	}

	public IShaderProgram Create(string vertexName, string fragmentName)
	{
		ShaderSource s1 = shaderSourceFactory.Create(vertexName);
		ShaderSource s2 = shaderSourceFactory.Create(fragmentName);

		IShaderProgram shaderProgram = new OpenGLShaderProgram(s1, s2);
		return shaderProgram;
	}
}