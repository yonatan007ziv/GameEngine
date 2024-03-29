using GraphicsEngine.Components.Shared;

namespace GraphicsEngine.Services.Interfaces.Utils.Managers;

public interface IShaderManager
{
	bool GetShader(string shaderName, out Shader shaderProgram);
}