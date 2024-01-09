using GameEngine.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GameEngine;

internal class Program
{
	public static void Main()
	{
		StartGameEngine();
	}

	private static IServiceProvider BuildProvider()
		=> new ServiceRegisterer().BuildProvider();

	private static void StartGameEngine()
		=> BuildProvider().GetRequiredService<GameEngine>().Run();
}