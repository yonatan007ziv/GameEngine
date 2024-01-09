using GameEngine.Core.API;

namespace GraphicsEngine;

public class Program
{
	public static void Main()
	{
		// Mock IPC Locally
		var renderer = GraphicsEngine.Start();
		while (true)
		{
			renderer.RenderFrame();
		}
	}
}