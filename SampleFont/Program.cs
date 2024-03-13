using GameEngine;
using GameEngine.Core.Components.Font;
using GameEngine.Core.SharedServices.Implementations;
using GameEngine.Core.SharedServices.Implementations.FileReaders;
using GameEngine.Core.SharedServices.Implementations.Loggers;
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
		GameEngine.FpsCap = 60;
		GameEngine.LogFps = false;
		GameEngine.LogInputs = false;
		GameEngine.LogRenderingLogs = true;
		GameEngine.MouseLocked = true;

		new FontTestingScene().LoadScene();

		GameEngine.MouseLocked = false;
		GameEngine.Run();
	}
}