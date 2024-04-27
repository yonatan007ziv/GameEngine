using GameEngine;
using GameEngine.Services.Interfaces;
using System.Drawing;

namespace BoxColliderSample;

internal class Program
{
	public static void Main()
	{
		IGameEngine GameEngine = new GameEngineProvider().UseSilkOpenGL().BuildEngine();

		GameEngine.SetResourceFolder(@$"{Directory.GetCurrentDirectory()}\Resources");
		GameEngine.SetWindowBackgroundColor(Color.LightBlue);
		GameEngine.Title = "Game Engine Sample - Box Colliders";
		GameEngine.FpsCap = 120;
		GameEngine.LogRenderingLogs = true;
		GameEngine.MouseLocked = true;

		new MainScene().LoadScene();

		GameEngine.Run();
	}
}