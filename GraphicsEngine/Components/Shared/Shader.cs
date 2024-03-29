using GraphicsEngine.Components.Interfaces;
using System.Numerics;

namespace GraphicsEngine.Components.Shared;

public class Shader
{
	public string ShaderName { get; }

	private readonly IShaderProgram shaderProgram;

	public Shader(IShaderProgram shaderProgram, string shaderName)
	{
		this.shaderProgram = shaderProgram;
		ShaderName = shaderName;
	}

	public void Bind()
		=> shaderProgram.Bind();
	public void Unbind()
		=> shaderProgram.Unbind();
	public void SetMatrix4Uniform(Matrix4x4 value, string uniformName)
		=> shaderProgram.SetMatrix4Uniform(value, uniformName);
}