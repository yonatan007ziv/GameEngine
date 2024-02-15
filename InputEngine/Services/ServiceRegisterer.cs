using GameEngine.Core.API;
using GameEngine.Core.SharedServices.Implementations.Loggers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InputEngine.Services;

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
		collection.AddSingleton<IInputEngine, Implementations.InputEngine>();
		collection.AddSingleton<ILogger, ConsoleLogger>();
	}
}