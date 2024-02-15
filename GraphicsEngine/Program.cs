using GameEngine.Core.API;
using GameEngine.Core.Enums;

namespace GraphicsEngine;

public class Program
{
	public static void Main()
	{
		GraphicsApi graphicsApi = GraphicsApi.SilkOpenGL;

		IGraphicsEngine renderer = GraphicsEngineProvider.BuildEngine(graphicsApi);
		renderer.Start();

		while (true)
			renderer.RenderFrame();
	}
}