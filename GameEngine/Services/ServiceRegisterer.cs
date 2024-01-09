using GameEngine.Core.Components;
using GameEngine.Core.SharedServices.Implementations;
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

	private void RegisterServices()
	{
		collection.AddSingleton<ILogger, ConsoleLogger>();

		RegisterEngines();

		RegisterFactories();
		RegisterManagers();
	}

	public IServiceProvider BuildProvider()
		=> collection.BuildServiceProvider();

	// TODO: find a way to register only required services for each engine.
	// No optional such as ILogger(s) that can be registered in multiple locations.
	// Avoid service duplication!
	private void RegisterEngines()
	{
		collection.AddSingleton<GameEngine>();

		// if (openGL)
		_ = new GraphicsRenderer.Services.ServiceRegisterer(collection);

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