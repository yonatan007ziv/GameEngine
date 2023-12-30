using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;
namespace GraphicsRenderer.Services.Implementations.Shared.Managers;

internal class ShaderManager : IShaderManager
{
	private readonly List<IShaderProgram> _registeredShaders = new List<IShaderProgram>();
	private readonly IShaderBank shaderBank;

	public IShaderProgram ActiveShader { get; private set; }

	public ShaderManager(IShaderBank shaderBank)
	{
		this.shaderBank = shaderBank;
		ActiveShader = null!;
	}

	public void RegisterShaders()
		=> shaderBank.RegisterShaders(this);

	public IShaderProgram GetShader(string shaderName) => shaderBank.GetShader(shaderName);

	public void RegisterShader(IShaderProgram shader)
	{
		if (!IsShaderRegistered(shader))
			_registeredShaders.Add(shader);
	}

	public void UnregisterShader(IShaderProgram shader)
	{
		if (IsShaderRegistered(shader))
			_registeredShaders.Remove(shader);
	}

	private bool IsShaderRegistered(IShaderProgram shader)
		=> _registeredShaders.Contains(shader);

	public void DisposeAll()
	{
		foreach (IShaderProgram sP in _registeredShaders)
			sP.Dispose();
	}
}