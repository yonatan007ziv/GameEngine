using Microsoft.Extensions.DependencyInjection;
using OpenGLRenderer.Services.Implementations.OpenGL.ModelImporters;
using OpenGLRenderer.Services.Implementations.OpenGL.Renderer;
using OpenGLRenderer.Services.Implementations.Utils;
using OpenGLRenderer.Services.Interfaces.OpenGL;
using OpenGLRenderer.Services.Interfaces.Utils;

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

		collection.AddSingleton<ISettingsManager, MockSettingsManager>();
		collection.AddSingleton<ITextureLoader, StbTextureLoader>();

		RegisterModelImporters();

		return collection.BuildServiceProvider();
	}

	private void RegisterModelImporters()
	{
		collection.AddSingleton<IModelImporter, ModelImporter>();
		collection.AddSingleton<ObjModelImporter>();
	}
}