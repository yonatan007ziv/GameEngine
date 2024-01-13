using GameEngine.Core.API;
using GraphicsEngine.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GraphicsEngine;

public class GraphicsEngineProvider
{
	public static IGraphicsEngine BuildEngine()
		=> new ServiceRegisterer().BuildProvider().GetRequiredService<IGraphicsEngine>();
}