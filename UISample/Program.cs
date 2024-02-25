using GameEngine;
using GameEngine.Core.Enums;
using GameEngine.Services.Interfaces;
using System.Drawing;
using UISample.Scenes;

namespace UISample;

internal class Program
{
	public static void Main()
	{
		// TrueTypeFontFileReader trueTypeFontFileReader = new TrueTypeFontFileReader(new ConsoleLogger(), new ResourceDiscoverer());
		// trueTypeFontFileReader.ReadFile("Arial.ttf", out Font font);

		IGameEngine GameEngine = GameEngineProvider.BuildEngine(GraphicsApi.SilkOpenGL);

		GameEngine.SetBackgroundColor(Color.LightBlue);
		GameEngine.Title = "Game Engine Sample - UI";
		GameEngine.FpsCap = 144; // Not supported for SilkOpenGL yet
		GameEngine.LogFps = false;
		GameEngine.LogInputs = false;
		GameEngine.LogRenderingLogs = false; // Not supported for OpenTK yet
		GameEngine.MouseLocked = false;

		new UIScene().LoadScene();

		GameEngine.Run();
	}
}