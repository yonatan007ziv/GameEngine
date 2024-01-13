using GameEngine.Core.API;
using GameEngine.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GameEngine;

public class GameEngineProvider
{
	public static IGameEngine BuildEngine()
		=> new ServiceRegisterer().BuildProvider().GetRequiredService<IGameEngine>();
}