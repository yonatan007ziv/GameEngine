using OpenGLRenderer.OpenGL;
using OpenGLRenderer.Resources.Shaders.Managed;
using OpenGLRenderer.Services.Interfaces.Utils.Managers;

namespace OpenGLRenderer.Services.Implementations.Utils.Managers;

internal class ShaderManager : IShaderManager
{
	private readonly List<ShaderProgram> _registeredShaders = new List<ShaderProgram>();
	public ShaderProgram ActiveShader { get; private set; }

    public ShaderManager()
    {
		
	}

    public void RegisterShader(ShaderProgram shader)
	{
		if (!IsShaderRegistered(shader))
			_registeredShaders.Add(shader);
	}

	public void UnregisterShader(ShaderProgram shader)
	{
		if (IsShaderRegistered(shader))
			_registeredShaders.Remove(shader);
	}

	public void BindShader(ShaderProgram shader)
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

	public void UnbindShader(ShaderProgram shader)
	{
		if (!IsShaderRegistered(shader))
			_registeredShaders.Add(shader);

		if (ActiveShader == shader)
		{
			shader.Unbind();
			ActiveShader = null;
		}
	}

	private bool IsShaderRegistered(ShaderProgram shader)
		=> _registeredShaders.Contains(shader);

	public void DisposeAll()
	{
		foreach (ShaderProgram sP in _registeredShaders)
			sP.Delete();
	}
}