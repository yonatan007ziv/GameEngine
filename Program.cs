using Microsoft.Extensions.DependencyInjection;
using OpenGLRenderer.Services;
using OpenGLRenderer.Services.Implementations.OpenGL.Renderer;

namespace OpenGLRenderer;

internal class Program
{
	public static void Main()
	{
		IServiceProvider provider =
			new ServiceRegisterer(new ServiceCollection()).BuildProvider();

		provider.GetRequiredService<Renderer>().Run();
	}
}