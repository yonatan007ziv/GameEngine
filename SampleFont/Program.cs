using GameEngine;
using GameEngine.Services.Interfaces;
using System.Drawing;

namespace SampleFont;

internal class Program
{
	public static void Main()
	{
		IGameEngine GameEngine = new GameEngineProvider().UseSilkOpenGL().BuildEngine();

		GameEngine.SetResourceFolder(@$"{Directory.GetCurrentDirectory()}\Resources");
		GameEngine.SetBackgroundColor(Color.LightBlue);
		GameEngine.Title = "Game Engine Sample - Sample Font";
		GameEngine.FpsCap = 144;
		GameEngine.LogRenderingLogs = true;

		new FontTestingScene().LoadScene();

		GameEngine.Run();
	}
}