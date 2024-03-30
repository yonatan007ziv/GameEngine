using GraphicsEngine.Components.Extensions;
using GraphicsEngine.Components.Interfaces;
using GraphicsEngine.Components.Shared;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Numerics;

namespace GraphicsEngine.Components.RendererSpecific.OpenTK;

public class OpenTKShaderProgram : IShaderProgram
{
	public int Id { get; private set; }

	public OpenTKShaderProgram(ShaderSource vShader, ShaderSource fShader)
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

	#region Dispose pattern
	private bool disposedValue;
	~OpenTKShaderProgram()
	{
		Dispose(disposing: false);
	}
	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{

			}

			Unbind();
			GL.DeleteProgram(Id);

			disposedValue = true;
		}
	}
	#endregion

	public void SetMatrix4Uniform(Matrix4x4 value, string uniformName)
	{
		Matrix4 matrix = value.ToOpenTK();

		int loc = GL.GetUniformLocation(Id, uniformName);
		GL.UniformMatrix4(loc, true, ref matrix);
	}

	public void SetFloat4Uniform(System.Numerics.Vector4 value, string uniformName)
	{
		int loc = GL.GetUniformLocation((uint)Id, uniformName);
		GL.Uniform4(loc, value.X, value.Y, value.Z, value.W);
	}
}