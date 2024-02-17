using GameEngine;
using GameEngine.Core.Enums;
using GameEngine.Services.Interfaces;
using Sample1.Scenes;
using System.Drawing;

namespace Sample1;

internal class Program
{
	public static void Main()
	{
		IGameEngine GameEngine = GameEngineProvider.BuildEngine(GraphicsApi.SilkOpenGL);

		GameEngine.SetBackgroundColor(Color.LightBlue);
		GameEngine.Title = "Game Engine Sample #1";
		GameEngine.FpsCap = 144;
		GameEngine.LogFps = false;
		GameEngine.LogInputs = false;
		GameEngine.LogRenderingLogs = false; // Not supported for OpenTK yet
		GameEngine.MouseLocked = true;

		new SingleCameraScene().LoadScene(); // One player scene
		// new SplitScreenScene().LoadScene(); // Two player scene, 1st player mouse and keyboard, 2nd player controller

		GameEngine.Run();
	}
}