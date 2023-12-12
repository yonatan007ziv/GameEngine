using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenGLRenderer.Components;
using OpenGLRenderer.Models;
using OpenGLRenderer.OpenGL;
using OpenGLRenderer.Services.Implementations.OpenGL.ModelImporters;
using OpenGLRenderer.Services.Implementations.OpenGL.Renderer;
using OpenGLRenderer.Services.Implementations.Utils;
using OpenGLRenderer.Services.Implementations.Utils.Factories;
using OpenGLRenderer.Services.Implementations.Utils.Managers;
using OpenGLRenderer.Services.Interfaces.OpenGL;
using OpenGLRenderer.Services.Interfaces.Utils;
using OpenGLRenderer.Services.Interfaces.Utils.Factories;
using OpenGLRenderer.Services.Interfaces.Utils.Managers;

namespace OpenGLRenderer.Services;

internal class ServiceRegisterer
{
	private readonly IServiceCollection collection;

	public ServiceRegisterer(IServiceCollection collection)
	{
		this.collection = collection;
	}

	public IServiceProvider BuildProvider()
	{
		collection.AddSingleton<Renderer>();

		collection.AddSingleton<ILogger, ConsoleLogger>();

		collection.AddSingleton<IFileReader<string>, StringFileReader>();
		collection.AddSingleton<ITextureLoader, StbTextureLoader>();
		collection.AddSingleton<IShaderBank, ShaderBank>();
		collection.AddTransient<IPerformanceAnalyzer, PerformanceAnalyzer>();

		RegisterManagers();
		RegisterFactories();
		RegisterModelImporters();

		return collection.BuildServiceProvider();
	}

	private void RegisterManagers()
	{
		collection.AddSingleton<IResourceManager, ContentResourceManager>();
		// collection.AddSingleton<IResourceManager, EmbeddedResourceManager>(); // Test in the Future

		collection.AddSingleton<ISettingsManager, MockSettingsManager>();
		collection.AddSingleton<IShaderManager, ShaderManager>(provider => new ShaderManager(provider.GetRequiredService<IShaderBank>()));
		collection.AddSingleton<ISceneManager, SceneManager>();
		collection.AddSingleton<IGameObjectManager, GameObjectManager>();
	}

	private void RegisterFactories()
	{
		collection.AddSingleton<IFactory<string, string, ShaderProgram>, ShaderProgramFactory>();
		collection.AddSingleton<IFactory<string, ShaderSource>, ShaderSourceFactory>();
		collection.AddSingleton<IFactory<Scene>, SceneFactory>();
		collection.AddSingleton<IFactory<GameObject>, GameObjectFactory>();
	}

	private void RegisterModelImporters()
	{
		collection.AddSingleton<IModelImporter, ModelImporter>();
		collection.AddSingleton<ObjModelImporter>();
		collection.AddSingleton<FbxModelImporter>();
	}
}