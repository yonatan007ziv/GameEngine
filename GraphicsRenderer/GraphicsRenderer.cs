using GraphicsRenderer.Services;
using GraphicsRenderer.Services.Interfaces.Renderer;
using Microsoft.Extensions.DependencyInjection;

namespace GraphicsRenderer;

public class GraphicsRenderer
{
	public static void Run()
	{
		new ServiceRegisterer()
			.BuildProvider()
			.GetRequiredService<IRenderer>()
			.Run();
	}
}