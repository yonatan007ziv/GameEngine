using GraphicsEngine.Components.Extensions;
using GraphicsEngine.Components.Interfaces;
using GraphicsEngine.Components.Shared;
using Silk.NET.OpenGL;
using System.Numerics;

namespace GraphicsEngine.Components.RendererSpecific.SilkOpenGL;

internal class SilkOpenGLShaderProgram : IShaderProgram
{
	private readonly GL glContext;

	public int Id { get; private set; }

	public SilkOpenGLShaderProgram(ShaderSource vShader, ShaderSource fShader, GL glContext)
	{
		this.glContext = glContext;

		uint id = glContext.CreateProgram();

		uint vId = glContext.CreateShader(ShaderType.VertexShader);
		uint fId = glContext.CreateShader(ShaderType.FragmentShader);

		glContext.ShaderSource(vId, vShader.Source);
		glContext.ShaderSource(fId, fShader.Source);

		glContext.CompileShader(vId);
		glContext.CompileShader(fId);

		glContext.AttachShader(id, vId);
		glContext.AttachShader(id, fId);

		glContext.LinkProgram(id);

		glContext.DetachShader(id, vId);
		glContext.DetachShader(id, fId);

		glContext.DeleteShader(vId);
		glContext.DeleteShader(fId);

		Id = (int)id;
	}

	public void Bind()
	{
		glContext.UseProgram((uint)Id);
	}

	public void Unbind()
	{
		glContext.UseProgram(0);
	}

	public void SetMatrix4Uniform(Matrix4x4 value, string uniformName)
	{
		ReadOnlySpan<float> span = value.ToReadOnlySpan();

		int loc = glContext.GetUniformLocation((uint)Id, uniformName);
		glContext.UniformMatrix4(loc, true, span);
	}

	#region Dispose pattern
	private bool disposedValue;
	~SilkOpenGLShaderProgram()
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
			glContext.DeleteProgram((uint)Id);

			disposedValue = true;
		}
	}
	#endregion
}