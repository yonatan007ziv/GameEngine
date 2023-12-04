using OpenTK.Graphics.OpenGL4;

namespace OpenGLRenderer.OpenGL;

internal class ShaderProgram : IDisposable
{
	public int Id { get; private set; }

	public ShaderProgram(ShaderSource vShader, ShaderSource fShader)
	{
		Id = GL.CreateProgram();

		int vId = GL.CreateShader(ShaderType.VertexShader);
		int fId = GL.CreateShader(ShaderType.FragmentShader);

		GL.ShaderSource(vId, vShader.GetSource());
		GL.ShaderSource(fId, fShader.GetSource());

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

	public void Delete()
		=> Dispose();

	public void Dispose()
	{
		GL.DeleteProgram(Id);
	}
}