using OpenGLRenderer.OpenGL;

namespace OpenGLRenderer.Services.Interfaces.Utils.Managers;

internal interface IShaderManager
{
	public ShaderProgram ActiveShader { get; }
	void RegisterShader(ShaderProgram shader);
	void UnregisterShader(ShaderProgram shader);
	void BindShader(ShaderProgram shader);
	void UnbindShader(ShaderProgram shader);
	void DisposeAll();
}