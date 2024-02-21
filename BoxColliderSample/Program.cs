using GameEngine;
using GameEngine.Core.Enums;
using GameEngine.Services.Interfaces;
using System.Drawing;

namespace BoxColliderSample;

internal class Program
{
	public static void Main()
	{
		IGameEngine GameEngine = GameEngineProvider.BuildEngine(GraphicsApi.SilkOpenGL);

		GameEngine.SetBackgroundColor(Color.LightBlue);
		GameEngine.Title = "Game Engine Sample - Box Colliders";
		GameEngine.FpsCap = 144;
		GameEngine.LogFps = false;
		GameEngine.LogInputs = false;
		GameEngine.LogRenderingLogs = true;
		GameEngine.MouseLocked = true;

		new MainScene().LoadScene();

		GameEngine.Run();
	}
}