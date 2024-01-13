using GameEngine.Components.GameObjectComponents;
using GameEngine.Core.API;
using GameEngine.Core.SharedServices.Implementations.Loggers;
using GameEngine.Core.SharedServices.Interfaces;
using GameEngine.Services.Implementations.Factories;
using GameEngine.Services.Implementations.Managers;
using GameEngine.Services.Interfaces.Managers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GameEngine.Services;

internal class ServiceRegisterer
{
	private readonly IServiceCollection collection = new ServiceCollection();

	public ServiceRegisterer()
	{
		RegisterServices();
	}

	public IServiceProvider BuildProvider()
		=> collection.BuildServiceProvider();

	private void RegisterServices()
	{
		collection.AddSingleton<ILogger, ConsoleLogger>();

		RegisterEngines();

		RegisterFactories();
		RegisterManagers();
	}

	private void RegisterEngines()
	{
		collection.AddSingleton<IGameEngine, Implementations.GameEngine>();

		collection.AddSingleton<IGraphicsEngine>(provider => GraphicsEngine.GraphicsEngineProvider.BuildEngine());
		collection.AddSingleton<IPhysicsEngine>(provider => PhysicsEngine.PhysicsEngineProvider.BuildEngine());

		// Physics Engine
		// Sound Engine
		// Input Engine (maybe)
	}

	private void RegisterFactories()
	{
		collection.AddSingleton<IFactory<int, GameObject>, GameObjectFactory>();
	}

	private void RegisterManagers()
	{
		collection.AddSingleton<IGameObjectManager, GameObjectManager>();
	}
}