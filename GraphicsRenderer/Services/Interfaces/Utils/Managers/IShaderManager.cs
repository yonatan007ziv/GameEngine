using GraphicsRenderer.Components.Interfaces;

namespace GraphicsRenderer.Services.Interfaces.Utils.Managers;

internal interface IShaderManager
{
	public IShaderProgram ActiveShader { get; }
	void RegisterShaders();
	IShaderProgram GetShader(string shaderName);
	void RegisterShader(IShaderProgram shader);
	void UnregisterShader(IShaderProgram shader);
	void DisposeAll();
}