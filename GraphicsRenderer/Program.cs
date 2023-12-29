using GraphicsRenderer.Services;
using GraphicsRenderer.Services.Interfaces.Renderer;
using Microsoft.Extensions.DependencyInjection;

namespace GraphicsRenderer
{
    internal class Program
	{
		static void Main(string[] args)
		{
			IServiceProvider provider =
				new ServiceRegisterer(new ServiceCollection()).BuildProvider();

			provider.GetRequiredService<IRenderer>().Run();
		}
	}
}