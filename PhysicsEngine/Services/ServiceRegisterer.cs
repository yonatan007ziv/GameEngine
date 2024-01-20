using GameEngine.Core.API;
using Microsoft.Extensions.DependencyInjection;

namespace PhysicsEngine.Services;

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
		collection.AddSingleton<IPhysicsEngine, Implementations.PhysicsEngine>();
	}
}