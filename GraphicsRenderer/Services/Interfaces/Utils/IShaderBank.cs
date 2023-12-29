using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;

namespace GraphicsRenderer.Services.Interfaces.Utils;

internal interface IShaderBank
{
	void RegisterShaders(IShaderManager shaderManager);
	IShaderProgram GetShader(string shaderName);
}