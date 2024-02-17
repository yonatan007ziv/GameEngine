using GameEngine.Services.Interfaces;
using System.Drawing;

namespace UISample;

internal class Program
{
	public static void Main()
	{
		IGameEngine gameEngine = GameEngine.GameEngineProvider.BuildEngine(GameEngine.Core.Enums.GraphicsApi.SilkOpenGL);

		gameEngine.Run();
	}
}