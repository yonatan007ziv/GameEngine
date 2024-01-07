using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Components.Shared;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace GraphicsRenderer.Components.OpenGL;

public class OpenGLShaderProgram : IShaderProgram
{
	public int Id { get; private set; }

	public OpenGLShaderProgram(ShaderSource vShader, ShaderSource fShader)
	{
		Id = GL.CreateProgram();

		int vId = GL.CreateShader(ShaderType.VertexShader);
		int fId = GL.CreateShader(ShaderType.FragmentShader);

		GL.ShaderSource(vId, vShader.Source);
		GL.ShaderSource(fId, fShader.Source);

		GL.CompileShader(vId);
		GL.CompileShader(fId);

		GL.AttachShader(Id, vId);
		GL.AttachShader(Id, fId);

		GL.LinkProgram(Id);

		GL.DeleteShader(vId);
		GL.DeleteShader(fId);
	}

	public void Bind()
	{
		GL.UseProgram(Id);
	}

	public void Unbind()
	{
		GL.UseProgram(0);
	}

	public void Dispose()
	{
		GL.DeleteProgram(Id);
	}

	public void SetMatrix4Uniform(ref Matrix4 value, string uniformName)
	{
		int loc = GL.GetUniformLocation(Id, uniformName);
		GL.UniformMatrix4(loc, true, ref value);
	}
}