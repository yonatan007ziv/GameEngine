using GameEngine.Core.API;
using GameEngine.Core.Enums;
using GameEngine.Core.SharedServices.Implementations.Loggers;
using GameEngine.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GameEngine.Services;

internal class ServiceRegisterer
{
	private readonly IServiceCollection collection = new ServiceCollection();

	public ServiceRegisterer(GraphicsApi graphicsApi)
	{
		RegisterServices(graphicsApi);
	}

	public IServiceProvider BuildProvider()
		=> collection.BuildServiceProvider();

	private void RegisterServices(GraphicsApi graphicsApi)
	{
		collection.AddSingleton<ILogger, ConsoleLogger>();

		RegisterEngines(graphicsApi);
	}

	private void RegisterEngines(GraphicsApi graphicsApi)
	{
		collection.AddSingleton<IGameEngine, Implementations.GameEngine>();

		collection.AddSingleton<IGraphicsEngine>(provider => GraphicsEngine.GraphicsEngineProvider.BuildEngine(graphicsApi));
		collection.AddSingleton<IPhysicsEngine>(provider => PhysicsEngine.PhysicsEngineProvider.BuildEngine());
		collection.AddSingleton<ISoundEngine>(provider => SoundEngine.SoundEngineProvider.BuildEngine());
		collection.AddSingleton<IInputEngine>(provider => InputEngine.InputEngineProvider.BuildEngine());
	}
}