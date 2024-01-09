using GameEngine.Core.API;
using GraphicsEngine.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GraphicsEngine;

public class GraphicsEngine
{
	public static IGraphicsEngine Start()
	{
		IGraphicsEngine renderer = new ServiceRegisterer().BuildProvider().GetRequiredService<IGraphicsEngine>();
		renderer.Start();
		return renderer;
	}
}