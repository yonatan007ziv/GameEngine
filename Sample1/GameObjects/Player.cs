using GameEngine.Components.Objects.Scriptable;
using GameEngine.Core.Components.Input.Buttons;
using Sample1.Components;
using System.Drawing;

namespace Sample1.GameObjects;

internal class Player : ScriptableWorldObject
{
	public readonly MovementController movementController;
	public readonly WorldCamera camera;

	public Player(PlayerMovementControls movementControls, AxesSet cameraAxes, bool singlePlayerSceneLoader)
	{
		_ = new SceneSwitcher(this, singlePlayerSceneLoader);
		movementController = new MovementController(this, movementControls);
		camera = new WorldCamera(this, cameraAxes);
	}

	public override void Update(float deltaTime)
	{
		if (GetKeyboardButtonDown(KeyboardButton.One))
			SetBackgroundColor(Color.Red);
		if (GetKeyboardButtonDown(KeyboardButton.Two))
			SetBackgroundColor(Color.Green);
		if (GetKeyboardButtonDown(KeyboardButton.Three))
			SetBackgroundColor(Color.Blue);
	}
}