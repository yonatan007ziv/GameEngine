using OpenGLRenderer.OpenGL;
using OpenGLRenderer.Services.Interfaces.Utils;
using OpenGLRenderer.Services.Interfaces.Utils.Factories;
using OpenGLRenderer.Services.Interfaces.Utils.Managers;

namespace OpenGLRenderer.Services.Implementations.Utils;

internal class ShaderBank : IShaderBank
{
	private readonly IFactory<string, string, ShaderProgram> shaderProgramFactory;

	private ShaderProgram? DefaultShader;
	private ShaderProgram? GizmosShader;

	public ShaderBank(IFactory<string, string, ShaderProgram> shaderProgramFactory)
	{
		this.shaderProgramFactory = shaderProgramFactory;
	}

	public void RegisterShaders(IShaderManager shaderManager)
	{
		DefaultShader = shaderProgramFactory.Create("DefVertex.glsl", "DefFragment.glsl");
		GizmosShader = shaderProgramFactory.Create("GizmosVertex.glsl", "GizmosFragment.glsl");

		shaderManager.RegisterShader(DefaultShader);
		shaderManager.RegisterShader(GizmosShader);
	}

	public ShaderProgram GetDefaultShader() => DefaultShader!;
	public ShaderProgram GetGizmosShader() => GizmosShader!;
}