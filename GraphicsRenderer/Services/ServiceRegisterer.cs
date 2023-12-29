using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Components.OpenGL;
using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Services.Implementations.Mocks;
using GraphicsRenderer.Services.Implementations.Utils;
using GraphicsRenderer.Services.Implementations.Utils.Factories;
using GraphicsRenderer.Services.Implementations.Utils.Factories.Shaders;
using GraphicsRenderer.Services.Implementations.Utils.Managers;
using GraphicsRenderer.Services.Implementations.Utils.ModelImporter;
using GraphicsRenderer.Services.Interfaces.Renderer;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenGLRenderer.Services.Implementations.Utils;

namespace GraphicsRenderer.Services;

internal class ServiceRegisterer
{
	private IServiceCollection collection;

	public ServiceRegisterer(IServiceCollection collection)
	{
		this.collection = collection;
	}

	public IServiceProvider BuildProvider()
	{
		RegisterOpenGL();
		RegisterShared();

		return collection.BuildServiceProvider();
	}

	private void RegisterOpenGL()
	{
		collection.AddSingleton<IRenderer, Implementations.Renderer.OpenGL.OpenGLRenderer>();
		collection.AddSingleton<IBufferGenerator, OpenGLBufferGenerator>();
	}

	private void RegisterShared()
	{
		collection.AddSingleton<ILogger, ConsoleLogger>();
		collection.AddSingleton<IFileReader<string>, StringFileReader>();
		collection.AddSingleton<ITextureLoader, StbTextureLoader>();
		collection.AddSingleton<IShaderBank, ShaderBank>();
		collection.AddTransient<IPerformanceAnalyzer, PerformanceAnalyzer>();
		collection.AddSingleton<IModelImporter, ModelImporter>();
		collection.AddSingleton<ObjModelImporter>();
		collection.AddSingleton<FbxModelImporter>();

		// Managers
		collection.AddSingleton<ISettingsManager, MockSettingsManager>();
		collection.AddSingleton<IShaderManager, ShaderManager>(provider => new ShaderManager(provider.GetRequiredService<IShaderBank>()));
		collection.AddSingleton<ISceneManager, SceneManager>();
		collection.AddSingleton<IGameObjectManager, GameObjectManager>();
		// collection.AddSingleton<IResourceManager, EmbeddedResourceManager>(); // Test in the Future
		collection.AddSingleton<IResourceManager, ContentResourceManager>();

		// Factories
		collection.AddSingleton<IFactory<string, string, IShaderProgram>, ShaderProgramFactory>();
		collection.AddSingleton<IFactory<string, ShaderSource>, ShaderSourceFactory>();
		collection.AddSingleton<IFactory<Scene>, SceneFactory>();
		collection.AddSingleton<IFactory<GameObject>, GameObjectFactory>();
	}
}