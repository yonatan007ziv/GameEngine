using GraphicsEngine.Components.Interfaces;
using OpenTK.Mathematics;

namespace GraphicsEngine.Components.Shared;

public class Shader
{
	private readonly IShaderProgram shaderProgram;

	public Shader(IShaderProgram shaderProgram)
	{
		this.shaderProgram = shaderProgram;
	}

	public void Bind()
		=> shaderProgram.Bind();
	public void Unbind()
		=> shaderProgram.Unbind();
	public void SetMatrix4Uniform(Matrix4 value, string uniformName)
		=> shaderProgram.SetMatrix4Uniform(ref value, uniformName);
}