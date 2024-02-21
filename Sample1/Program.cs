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
		GameEngine.Title = "Game Engine Sample - #1";
		GameEngine.FpsCap = 60; // Not supported for SilkOpenGL yet
		GameEngine.LogFps = false;
		GameEngine.LogInputs = false;
		GameEngine.LogRenderingLogs = true; // Not supported for OpenTK yet
		GameEngine.MouseLocked = true;

		// Press 4 on the keyboard to switch to a splitscreen scene
		new SingleCameraScene().LoadScene();

		GameEngine.Run();
	}
}