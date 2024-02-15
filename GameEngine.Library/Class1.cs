using GameEngine.Core.API;

namespace GameEngine.Library;

public class Class1
{
	public static IGameEngine BuildGameEngine()
		=> GameEngineProvider.BuildEngine(Core.Enums.GraphicsApi.OpenTK);
	public static void Print()
		=> Console.WriteLine("Nigga");
}