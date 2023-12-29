﻿using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Components.Shared;
using OpenTK.Graphics.OpenGL4;

namespace GraphicsRenderer.Components.OpenGL;

internal class OpenGLShaderProgram : IShaderProgram
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

	[Obsolete("Don't use outside the IShaderManager")]
	public void Bind()
	{
		GL.UseProgram(Id);
	}

	[Obsolete("Don't use outside the IShaderManager")]
	public void Unbind()
	{
		GL.UseProgram(0);
	}

	public void Dispose()
	{
		GL.DeleteProgram(Id);
	}
}