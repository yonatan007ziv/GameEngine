using OpenGLRenderer.OpenGL;
using OpenGLRenderer.Services.Interfaces.Utils.Managers;

namespace OpenGLRenderer.Services.Interfaces.Utils;

internal interface IShaderBank
{
	void RegisterShaders(IShaderManager shaderManager);
	ShaderProgram GetDefaultShader();
	ShaderProgram GetGizmosShader();
}