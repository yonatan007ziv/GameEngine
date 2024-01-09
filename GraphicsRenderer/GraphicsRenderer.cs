using GameEngine.Core.API;
using GraphicsRenderer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GraphicsRenderer;

public class GraphicsRenderer
{
	public static IGraphicsEngine Start()
	{
		IGraphicsEngine renderer = new ServiceRegisterer().BuildProvider().GetRequiredService<IGraphicsEngine>();
		renderer.Start();
		return renderer;
	}
}