using GameEngine;
using GameEngine.Core.Components.Input.Buttons;
using GameEngine.Core.Enums;
using GameEngine.Services.Interfaces;

namespace Experimentation;

internal class Program
{
	public static void Main()
	{
		IGameEngine GameEngine = GameEngineProvider.BuildEngine(GraphicsApi.OpenTK);

		GameEngine.Title = "Game Engine Sample - Experimentation";
		GameEngine.MouseLocked = true;

		GameEngine.InputEngine.MapKeyboardAxis("KeyboardX", KeyboardButton.D, KeyboardButton.A, 1, 0);
		GameEngine.InputEngine.MapKeyboardAxis("KeyboardY", KeyboardButton.W, KeyboardButton.S, 1, 0);
		GameEngine.InputEngine.MapMouseAxis("MouseX", MouseAxis.MouseHorizontal, 1, 0);
		GameEngine.InputEngine.MapMouseAxis("MouseY", MouseAxis.MouseVertical, 1, 0);

		new OpenScene().LoadScene();

		GameEngine.Run();
	}
}