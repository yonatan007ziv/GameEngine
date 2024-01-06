using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Components.Interfaces.Buffers;
using GraphicsRenderer.Components.OpenGL;
using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Components.Shared.Data;
using GraphicsRenderer.Services.Implementations.Direct11.Renderer;
using GraphicsRenderer.Services.Implementations.OpenGL;
using GraphicsRenderer.Services.Implementations.OpenGL.Input;
using GraphicsRenderer.Services.Implementations.OpenGL.Renderer;
using GraphicsRenderer.Services.Implementations.Shared;
using GraphicsRenderer.Services.Implementations.Shared.Factories;
using GraphicsRenderer.Services.Implementations.Shared.Factories.Shaders;
using GraphicsRenderer.Services.Implementations.Shared.FileReaders;
using GraphicsRenderer.Services.Implementations.Shared.Managers;
using GraphicsRenderer.Services.Implementations.Shared.ModelImporter;
using GraphicsRenderer.Services.Interfaces.InputProviders;
using GraphicsRenderer.Services.Interfaces.Renderer;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GraphicsRenderer.Services;

public class ServiceRegisterer
{
	private readonly IServiceCollection collection = new ServiceCollection();

	public IServiceProvider BuildProvider()
	{
		RegisterOpenGL();
		// RegisterDirect11();

		RegisterShared();

		return collection.BuildServiceProvider();
	}

	private void RegisterOpenGL()
	{
		collection.AddSingleton<IRenderer, OpenGLRenderer>();
		collection.AddSingleton<IBufferGenerator, OpenGLBufferGenerator>();
		collection.AddSingleton<IFactory<string, IMesh>, OpenGLMeshFactory>();

		collection.AddSingleton<IInputProvider, OpenGLInputProvider>();
		collection.AddSingleton<IMouseInputProvider, OpenGLMouseInputProvider>();
		collection.AddSingleton<IKeyboardInputProvider, OpenGLKeyboardInputProvider>();
	}

	private void RegisterDirect11()
	{
		collection.AddSingleton<IRenderer, Direct11Renderer>();
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
		collection.AddSingleton<IGameObjectManager, GameObjectManager>();
		collection.AddSingleton<IResourceManager, ContentResourceManager>(); // EmbeddedResourceManager : Test in the Future
		collection.AddSingleton<ITextureManager, TextureManager>();

		// Factories
		collection.AddSingleton<IFactory<string, string, string, GameObject>, MaterializedGameObjectFactory>();
		collection.AddSingleton<IFactory<string, string, Material>, MaterialFactory>();
		collection.AddSingleton<IFactory<string, string, IShaderProgram>, ShaderProgramFactory>();
		collection.AddSingleton<IFactory<string, ShaderSource>, ShaderSourceFactory>();
		collection.AddSingleton<IFactory<string, ITextureBuffer>, TextureFactory>();
		collection.AddSingleton<IFactory<string, ModelData>, ModelFactory>();
		collection.AddSingleton<IFactory<IGameObjectManager, GameObject>, GameObjectFactory>();

		// File Readers
		collection.AddSingleton<IFileReader<string>, StringFileReader>();
		collection.AddSingleton<IFileReader<FileStream>, FileStreamFileReader>();
	}
}