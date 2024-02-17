using GameEngine.Components;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Buttons;
using Sample1.Components;
using Sample1.GameObjects;
using System.Numerics;

namespace Sample1.Scenes;

internal class SingleCameraScene : Scene
{
	public SingleCameraScene()
	{
		// Register keyboard / controller movement axes
		MapKeyboardAxis("XMovement", KeyboardButton.D, KeyboardButton.A, 1, 0);
		MapKeyboardAxis("YMovement", KeyboardButton.W, KeyboardButton.S, 1, 0);
		MapGamepadAxis("XMovement", GamepadAxis.LeftAnalogHorizontal, 1, 0);
		MapGamepadAxis("YMovement", GamepadAxis.LeftAnalogVertical, -1, 0);

		// Register mouse / controller look axes
		MapMouseAxis("XCamera", MouseAxis.MouseHorizontal, 1, 0);
		MapMouseAxis("YCamera", MouseAxis.MouseVertical, 1, 0);
		MapGamepadAxis("XCamera", GamepadAxis.RightAnalogHorizontal, 5, 0);
		MapGamepadAxis("YCamera", GamepadAxis.RightAnalogVertical, 5, 0);

		MapKeyboardButton("Jump", KeyboardButton.Space);
		MapGamepadButton("Jump", GamepadButton.Cross);

		MapKeyboardButton("Escape", KeyboardButton.Escape);
		MapGamepadButton("Escape", GamepadButton.Start);

		// Ground
		gameObjects.Add(new Ground(new Vector2(100, 100)));

		// Trex
		Trex trex = new Trex();
		trex.Transform.Scale /= 2;
		gameObjects.Add(trex);

		// Player
		Player player = new Player(new PlayerMovementControls(new AxesSet("XMovement", "YMovement"), "Jump", "Escape"), new AxesSet("XCamera", "YCamera"), false);
		player.Transform.Position += new Vector3(0, 10, -10);
		gameObjects.Add(player);
		cameras.Add((player.camera, new ViewPort(0.5f, 0.5f, 1, 1)));
	}
}