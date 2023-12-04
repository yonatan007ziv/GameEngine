using Microsoft.Extensions.DependencyInjection;
using OpenGLRenderer.Services.Implementations.OpenGL;
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
		collection.AddSingleton<IModelImporter, ModelImporter>();

		return collection.BuildServiceProvider();
	}
}