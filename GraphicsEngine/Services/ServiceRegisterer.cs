﻿using GameEngine.Core.API;
using GameEngine.Core.SharedServices.Implementations;
using GameEngine.Core.SharedServices.Implementations.FileReaders;
using GameEngine.Core.SharedServices.Implementations.Loggers;
using GameEngine.Core.SharedServices.Implementations.Managers;
using GameEngine.Core.SharedServices.Interfaces;
using GameEngine.Core.SharedServices.Interfaces.Utils.Managers;
using GraphicsEngine.Components.Interfaces;
using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.OpenGL;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Components.Shared.Data;
using GraphicsEngine.Services.Implementations.OpenGL;
using GraphicsEngine.Services.Implementations.OpenGL.Renderer;
using GraphicsEngine.Services.Implementations.Shared;
using GraphicsEngine.Services.Implementations.Shared.Factories;
using GraphicsEngine.Services.Implementations.Shared.Factories.Shaders;
using GraphicsEngine.Services.Implementations.Shared.Managers;
using GraphicsEngine.Services.Implementations.Shared.ModelImporter;
using GraphicsEngine.Services.Interfaces.Utils;
using GraphicsEngine.Services.Interfaces.Utils.Managers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GraphicsEngine.Services;

internal class ServiceRegisterer
{
	private readonly IServiceCollection collection;

	public ServiceRegisterer()
	{
		collection = new ServiceCollection();
		RegisterServices();
	}

	public IServiceProvider BuildProvider()
		=> collection.BuildServiceProvider();

	private void RegisterServices()
	{
		RegisterOpenGL();
		// RegisterDirect11();

		RegisterShared();
	}

	private void RegisterOpenGL()
	{
		collection.AddSingleton<IGraphicsEngine, OpenGLRenderer>();
		collection.AddSingleton<IBufferGenerator, OpenGLBufferGenerator>();
		collection.AddSingleton<IFactory<string, string, IMeshRenderer>, OpenGLMeshRendererFactory>();
	}

	private void RegisterDirect11()
	{
		// collection.AddSingleton<IGraphicsEngine, Direct11Renderer>();
		// collection.AddSingleton<IBufferGenerator, OpenGLBufferGenerator>();
	}

	private void RegisterShared()
	{
		collection.AddSingleton<ILogger, ConsoleLogger>();

		collection.AddSingleton<ITextureLoader, StbTextureLoader>();
		collection.AddTransient<IPerformanceAnalyzer, PerformanceAnalyzer>();

		collection.AddSingleton<ObjModelImporter>();
		collection.AddSingleton<FbxModelImporter>();

		// Managers
		collection.AddSingleton<IShaderManager, ShaderManager>();
		collection.AddSingleton<IResourceManager, ContentResourceManager>(); // EmbeddedResourceManager : Test in the Future
		collection.AddSingleton<ITextureManager, TextureManager>();

		// Factories
		collection.AddSingleton<IFactory<string, Material>, MaterialFactory>();
		collection.AddSingleton<IFactory<string, string, IShaderProgram>, ShaderProgramFactory>();
		collection.AddSingleton<IFactory<string, ShaderSource>, ShaderSourceFactory>();
		collection.AddSingleton<IFactory<string, ITextureBuffer>, TextureFactory>();
		collection.AddSingleton<IFactory<string, ModelData>, ModelFactory>();

		// File Readers
		collection.AddSingleton<IFileReader<string>, StringFileReader>();
		collection.AddSingleton<IFileReader<FileStream>, FileStreamFileReader>();
	}
}