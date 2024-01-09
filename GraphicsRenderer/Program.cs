using GameEngine.Core.API;

namespace GraphicsRenderer;

public class Program
{
	public static void Main()
	{
		// Mock IPC Locally
		var renderer = GraphicsRenderer.Start();
		while (true)
		{
			renderer.RenderFrame();
		}
	}
}