using GraphicsEngine.Components.Interfaces;

namespace GraphicsEngine.Services.Interfaces.Utils.Managers;

public interface IShaderManager
{
	bool GetShader(string shaderName, out IShaderProgram shaderProgram);
}