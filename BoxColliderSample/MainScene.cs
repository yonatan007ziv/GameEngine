using GameEngine.Components;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Buttons;
using System.Numerics;

namespace BoxColliderSample;

internal class MainScene : Scene
{
	public MainScene()
	{
		MapKeyboardAxis("XMovement", KeyboardButton.D, KeyboardButton.A, 1, 0);
		MapKeyboardAxis("YMovement", KeyboardButton.W, KeyboardButton.S, 1, 0);
		MapGamepadAxis("XMovement", GamepadAxis.LeftAnalogHorizontal, 1, 0);
		MapGamepadAxis("YMovement", GamepadAxis.LeftAnalogVertical, -1, 0);

		MapMouseAxis("XCamera", MouseAxis.MouseHorizontal, 1, 0);
		MapMouseAxis("YCamera", MouseAxis.MouseVertical, -1, 0);
		MapGamepadAxis("XCamera", GamepadAxis.RightAnalogHorizontal, 5, 0);
		MapGamepadAxis("YCamera", GamepadAxis.RightAnalogVertical, -5, 0);

		MapKeyboardButton("Jump", KeyboardButton.Space);
		MapGamepadButton("Jump", GamepadButton.Cross);

		MapKeyboardButton("Escape", KeyboardButton.Escape);
		MapGamepadButton("Escape", GamepadButton.Start);

		// Ground, temp solution
		WorldObjects.Add(new Ground(new Vector2(100, 100)));

		// Wall
		Wall wall = new Wall(new Vector3(100, 100, 1));
		wall.Transform.Position += new Vector3(0, 50, 10);
		WorldObjects.Add(wall);

		// Trex
		Trex trex = new Trex();
		WorldObjects.Add(trex);

		// Player
		Player player = new Player();
		player.Transform.Position += new Vector3(0, 10, 0);

		WorldObjects.Add(player);
		WorldCameras.Add((player.camera, new ViewPort(0.5f, 0.5f, 1, 1)));

		UICameras.Add((new UICamera(), new ViewPort(0.5f, 0.5f, 1, 1)));
		TestButton a = new TestButton();
		a.Transform.Scale /= 5;
		a.Transform.Position = new Vector3(0.9f - a.Transform.Scale.X, 0.9f - a.Transform.Scale.Y, 0);
		UIObjects.Add(a);
	}
}