namespace GameEngine;

internal class Program
{
	public static void Main()
		=> GameEngineProvider.BuildEngine(Core.Enums.GraphicsApi.SilkOpenGL).Run();
}