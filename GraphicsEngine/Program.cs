using GameEngine.Core.API;

namespace GraphicsEngine;

public class Program
{
	public static void Main()
	{
		IGraphicsEngine renderer = GraphicsEngineProvider.BuildEngine();
		while (true)
			renderer.RenderFrame();
	}
}