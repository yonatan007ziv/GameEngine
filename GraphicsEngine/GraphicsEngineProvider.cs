using GameEngine.Core.API;
using GameEngine.Core.Enums;
using GraphicsEngine.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GraphicsEngine;

public class GraphicsEngineProvider
{
	public static IGraphicsEngine BuildEngine(GraphicsApi graphicsApi)
		=> new ServiceRegisterer(graphicsApi).BuildProvider().GetRequiredService<IGraphicsEngine>();
}