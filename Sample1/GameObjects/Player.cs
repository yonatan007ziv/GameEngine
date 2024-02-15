using GameEngine.Components;
using GameEngine.Core.Components.Input.Buttons;
using Sample1.Components;
using System.Drawing;

namespace Sample1.GameObjects;

internal class Player : ScriptableGameObject
{
	public readonly MovementController movementController;
	public readonly Camera camera;

	public Player(PlayerMovementControls movementControls, AxesSet cameraAxes)
	{
		movementController = new MovementController(Transform, movementControls);
		camera = new Camera(Transform, cameraAxes);
	}

	public override void Update(float deltaTime)
	{
		movementController.Update(deltaTime);
		camera.Update(deltaTime);

		if (GetKeyboardButtonDown(KeyboardButton.One))
			SetBackgroundColor(Color.Red);
		if (GetKeyboardButtonDown(KeyboardButton.Two))
			SetBackgroundColor(Color.Green);
		if (GetKeyboardButtonDown(KeyboardButton.Three))
			SetBackgroundColor(Color.Blue);
	}
}