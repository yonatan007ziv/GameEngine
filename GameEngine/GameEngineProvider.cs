using GameEngine.Services;
using GameEngine.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GameEngine;

public class GameEngineProvider
{
	public static IGameEngine BuildEngine(Core.Enums.GraphicsApi graphicsApi)
		=> new ServiceRegisterer(graphicsApi).BuildProvider().GetRequiredService<IGameEngine>();
}