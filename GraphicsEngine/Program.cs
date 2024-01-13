namespace GraphicsEngine;

public class Program
{
	public static void Main()
	{
		var renderer = GraphicsEngineProvider.BuildEngine();
		while (true)
		{
			renderer.RenderFrame();
		}
	}
}