using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;
namespace GraphicsRenderer.Services.Implementations.Shared.Managers;

public class ShaderManager : IShaderManager
{
	private readonly IFactory<string, string, IShaderProgram> shaderProgramFactory;
	private readonly Dictionary<string, IShaderProgram> shaders = new Dictionary<string, IShaderProgram>();

	public ShaderManager(IFactory<string, string, IShaderProgram> shaderProgramFactory)
	{
		this.shaderProgramFactory = shaderProgramFactory;
	}

	public bool GetShader(string shaderName, out IShaderProgram shaderProgram)
	{
		if (shaders.ContainsKey(shaderName))
		{
			shaderProgram = shaders[shaderName];
			return true;
		}
		else if (shaderProgramFactory.Create($"{shaderName}Vertex.glsl", $"{shaderName}Fragment.glsl", out shaderProgram))
		{
			shaders.Add(shaderName, shaderProgram);
			return true;
		}

		shaderProgram = default!;
		return false;
	}
}