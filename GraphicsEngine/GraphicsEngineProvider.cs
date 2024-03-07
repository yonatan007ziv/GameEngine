using GameEngine.Core.API;
using GraphicsEngine.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GraphicsEngine;

public class GraphicsEngineProvider
{
	public static IGraphicsEngine BuildOpenTKEngine()
		=> new ServiceRegisterer().RegisterOpenTK().BuildProvider().GetRequiredService<IGraphicsEngine>();
	public static IGraphicsEngine BuildSilkOpenGLEngine()
		=> new ServiceRegisterer().RegisterSilkOpenGL().BuildProvider().GetRequiredService<IGraphicsEngine>();

	public static void RegisterEngineOpenTK(IServiceCollection collection)
		=> new ServiceRegisterer(collection).RegisterOpenTK();
	public static void RegisterEngineSilkOpenGL(IServiceCollection collection)
		=> new ServiceRegisterer(collection).RegisterSilkOpenGL();
}