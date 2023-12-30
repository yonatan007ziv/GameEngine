using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;

namespace GraphicsRenderer.Services.Implementations.Shared;

internal class ShaderBank : IShaderBank
{
	private readonly IFactory<string, string, IShaderProgram> shaderProgramFactory;
	private readonly Dictionary<string, IShaderProgram> shaders = new Dictionary<string, IShaderProgram>();

	public ShaderBank(IFactory<string, string, IShaderProgram> shaderProgramFactory)
	{
		this.shaderProgramFactory = shaderProgramFactory;
	}

	public void RegisterShaders(IShaderManager shaderManager)
	{
		IShaderProgram Default = shaderProgramFactory.Create("DefVertex.glsl", "DefFragment.glsl");
		IShaderProgram Gizmos = shaderProgramFactory.Create("GizmosVertex.glsl", "GizmosFragment.glsl");
		IShaderProgram Textured = shaderProgramFactory.Create("DefVertex.glsl", "TexturedFragment.glsl");

		shaders.Add(nameof(Default), Default);
		shaders.Add(nameof(Gizmos), Gizmos);
		shaders.Add(nameof(Textured), Textured);

		shaderManager.RegisterShader(Default);
		shaderManager.RegisterShader(Gizmos);
		shaderManager.RegisterShader(Textured);
	}

	public IShaderProgram GetShader(string name) => shaders[name];
}