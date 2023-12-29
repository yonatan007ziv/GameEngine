using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;
namespace GraphicsRenderer.Services.Implementations.Utils.Managers;

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

	public void BindShader(IShaderProgram shader)
	{
		if (!IsShaderRegistered(shader))
			_registeredShaders.Add(shader);

		if (ActiveShader != shader)
		{
#pragma warning disable CS0618 // Permitted Usage
			shader.Bind();
#pragma warning restore CS0618

			ActiveShader = shader;
		}
	}

	public void UnbindShader(IShaderProgram shader)
	{
		if (!IsShaderRegistered(shader))
			_registeredShaders.Add(shader);

		if (ActiveShader == shader)
		{
#pragma warning disable CS0618 // Permitted Usage
			shader.Unbind();
#pragma warning restore CS0618

			BindShader(GetShader("Default"));
		}
	}

	private bool IsShaderRegistered(IShaderProgram shader)
		=> _registeredShaders.Contains(shader);

	public void DisposeAll()
	{
		foreach (IShaderProgram sP in _registeredShaders)
			sP.Dispose();
	}
}