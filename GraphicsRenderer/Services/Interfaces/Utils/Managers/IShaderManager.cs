using GraphicsRenderer.Components.Interfaces;

namespace GraphicsRenderer.Services.Interfaces.Utils.Managers;

public interface IShaderManager
{
	bool GetShader(string shaderName, out IShaderProgram shaderProgram);
}