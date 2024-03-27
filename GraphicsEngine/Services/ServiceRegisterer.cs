using GameEngine.Core.API;
using GameEngine.Core.Components.Fonts;
using GameEngine.Core.SharedServices.Implementations;
using GameEngine.Core.SharedServices.Implementations.FileReaders;
using GameEngine.Core.SharedServices.Implementations.Loggers;
using GameEngine.Core.SharedServices.Implementations.Managers;
using GameEngine.Core.SharedServices.Interfaces;
using GameEngine.Core.SharedServices.Interfaces.Utils.Managers;
using GraphicsEngine.Components.Interfaces;
using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Components.Shared.Data;
using GraphicsEngine.Services.Implementations.Direct11.Renderer;
using GraphicsEngine.Services.Implementations.Direct12;
using GraphicsEngine.Services.Implementations.OpenTK;
using GraphicsEngine.Services.Implementations.OpenTK.Renderer;
using GraphicsEngine.Services.Implementations.Shared;
using GraphicsEngine.Services.Implementations.Shared.Factories;
using GraphicsEngine.Services.Implementations.Shared.Factories.Shaders;
using GraphicsEngine.Services.Implementations.Shared.Managers;
using GraphicsEngine.Services.Implementations.Shared.ModelImporter;
using GraphicsEngine.Services.Implementations.SilkOpenGL;
using GraphicsEngine.Services.Implementations.SilkOpenGL.Renderer;
using GraphicsEngine.Services.Interfaces;
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
		RegisterShared();
	}

	public ServiceRegisterer(IServiceCollection collection)
	{
		this.collection = collection;
		RegisterShared();
	}

	public IServiceProvider BuildProvider()
		=> collection.BuildServiceProvider();

	public ServiceRegisterer RegisterOpenTK()
	{
		collection.AddSingleton<IInternalGraphicsRenderer, OpenTKRenderer>();
		collection.AddSingleton<IBufferSpecificGenerator, OpenTKBufferGenerator>();
		collection.AddSingleton<IBufferSpecificDeletor, OpenTKBufferDeletor>();
		collection.AddSingleton<IFactory<ShaderSource, ShaderSource, IShaderProgram>, OpenTKShaderProgramFactory>();

		collection.AddSingleton<IDrawingCall, OpenTKDrawingCall>();

		return this;
	}

	public ServiceRegisterer RegisterSilkOpenGL()
	{
		collection.AddSingleton<IInternalGraphicsRenderer, SilkOpenGLRenderer>();
		collection.AddSingleton<IBufferSpecificGenerator, SilkOpenGLBufferGenerator>();
		collection.AddSingleton<IBufferSpecificDeletor, SilkOpenGLBufferDeletor>();
		collection.AddSingleton<IFactory<ShaderSource, ShaderSource, IShaderProgram>, SilkOpenGLShaderProgramFactory>();

		collection.AddSingleton<IDrawingCall, SilkOpenGLDrawingCall>();

		return this;
	}

	private void RegisterSilkDirect11()
	{
		collection.AddSingleton<IInternalGraphicsRenderer, Direct11Renderer>();
	}
	private void RegisterSilkDirect12()
	{
		collection.AddSingleton<IInternalGraphicsRenderer, Direct12Renderer>();
	}

	private void RegisterShared()
	{
		collection.AddSingleton<IGraphicsEngine, Implementations.Shared.GraphicsEngine>();

		collection.AddSingleton<ILogger, ConsoleLogger>();

		collection.AddSingleton<ITextureLoader, StbTextureLoader>();
		collection.AddTransient<IPerformanceAnalyzer, PerformanceAnalyzer>();

		collection.AddSingleton<ObjModelImporter>();

		// Managers
		collection.AddSingleton<IShaderManager, GLShaderManager>();
		collection.AddSingleton<IResourceManager, ContentResourceManager>();
		collection.AddSingleton<IResourceDiscoverer, ResourceDiscoverer>();
		collection.AddSingleton<ITextureManager, TextureManager>();

		// Factories
		collection.AddSingleton<IFactory<string, Material>, MaterialFactory>();
		collection.AddSingleton<IFactory<string, string, IShaderProgram>, ShaderProgramFactory>();
		collection.AddSingleton<IFactory<string, ShaderSource>, ShaderSourceFactory>();
		collection.AddSingleton<IFactory<string, ITextureBuffer>, TextureFactory>();
		collection.AddSingleton<IFactory<string, ModelData>, ModelFactory>();

		// Font reader
		collection.AddSingleton<IFileReader<Font>, FontFileReader>();

		// Buffer managers
		collection.AddSingleton<IBufferDeletor, BufferDeletor>();
		collection.AddSingleton<IBufferFactory, BufferFactory>();

		// File Readers
		collection.AddSingleton<IFileReader<string>, StringFileReader>();
		collection.AddSingleton<IFileReader<FileStream>, FileStreamFileReader>();
	}
}