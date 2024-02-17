using GameEngine.Components;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Buttons;
using Sample1.Components;
using Sample1.GameObjects;
using System.Numerics;

namespace Sample1.Scenes;

internal class SplitScreenScene : Scene
{
	public SplitScreenScene()
	{
		// Player A inputs
		MapKeyboardAxis("XMovementP1", KeyboardButton.D, KeyboardButton.A, 1, 0);
		MapKeyboardAxis("YMovementP1", KeyboardButton.W, KeyboardButton.S, 1, 0);
		MapMouseAxis("XCameraP1", MouseAxis.MouseHorizontal, 1, 0);
		MapMouseAxis("YCameraP1", MouseAxis.MouseVertical, 1, 0);
		MapKeyboardButton("JumpP1", KeyboardButton.Space);
		MapKeyboardButton("EscapeP1", KeyboardButton.Escape);

		// Player B inputs
		MapGamepadAxis("XMovementP2", GamepadAxis.LeftAnalogHorizontal, 1, 0);
		MapGamepadAxis("YMovementP2", GamepadAxis.LeftAnalogVertical, -1, 0);
		MapGamepadAxis("XCameraP2", GamepadAxis.RightAnalogHorizontal, 5, 0);
		MapGamepadAxis("YCameraP2", GamepadAxis.RightAnalogVertical, 5, 0);
		MapGamepadButton("JumpP2", GamepadButton.Cross);
		MapGamepadButton("EscapeP2", GamepadButton.Start);

		// Ground
		worldObjects.Add(new Ground(new Vector2(100, 100)));

		// Trex
		Trex trex = new Trex();
		trex.Transform.Scale /= 2;
		worldObjects.Add(trex);

		// Players
		Player playerOne = new Player(new PlayerMovementControls(new AxesSet("XMovementP1", "YMovementP1"), "JumpP1", "EscapeP1"), new AxesSet("XCameraP1", "YCameraP1"), true);
		Player playerTwo = new Player(new PlayerMovementControls(new AxesSet("XMovementP2", "YMovementP2"), "JumpP2", "EscapeP2"), new AxesSet("XCameraP2", "YCameraP2"), true);

		playerOne.Transform.Position += new Vector3(0, 10, -10);
		playerTwo.Transform.Position += new Vector3(0, 10, 10);

		worldObjects.Add(playerOne);
		worldCameras.Add((playerOne.camera, new ViewPort(0.5f, 0.75f, 1, 0.5f)));

		worldObjects.Add(playerTwo);
		worldCameras.Add((playerTwo.camera, new ViewPort(0.5f, 0.25f, 1, 0.5f)));
	}
}