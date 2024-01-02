using GraphicsRenderer.Services;
using GraphicsRenderer.Services.Interfaces.Renderer;
using Microsoft.Extensions.DependencyInjection;

namespace GraphicsRenderer;

public class Program
{
	public static void Main()
	{
		new ServiceRegisterer()
			.BuildProvider()
			.GetRequiredService<IRenderer>()
			.Run();
	}
}