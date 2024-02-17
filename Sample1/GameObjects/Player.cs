using GameEngine.Components.Objects.Scriptable;
using GameEngine.Core.Components.Input.Buttons;
using Sample1.Components;
using System.Drawing;

namespace Sample1.GameObjects;

internal class Player : ScriptableWorldObject
{
	// Gravity, -10 was a little low
	private const float gravityMagnitude = 20;

	public readonly MovementController movementController;
	public readonly WorldCamera camera;

	private bool addedGravity;
	private bool addedNormal;

	public Player(PlayerMovementControls movementControls, AxesSet cameraAxes, bool singlePlayerSceneLoader)
	{
		_ = new SceneSwitcher(this, singlePlayerSceneLoader);
		movementController = new MovementController(this, movementControls);
		camera = new WorldCamera(this, cameraAxes);
	}

	public override void Update(float deltaTime)
	{
        // Simulate ground collision for now
        #region hardcoded garbage temporary
        if (Transform.Position.Y <= 1.5f && !addedNormal)
		{
			Forces.Clear();
			ImpulseVelocities.Clear();
			ImpulseVelocities.Add(new System.Numerics.Vector3(0));
			addedNormal = true;
			addedGravity = false;
		}
		else if (Transform.Position.Y > 1.5f)
		{
			if (!addedGravity)
			{
				Forces.Add(new System.Numerics.Vector3(0, -gravityMagnitude, 0));
				addedGravity = true;
			}
			addedNormal = false;
		}
		#endregion

		if (GetKeyboardButtonDown(KeyboardButton.One))
			SetBackgroundColor(Color.Red);
		if (GetKeyboardButtonDown(KeyboardButton.Two))
			SetBackgroundColor(Color.Green);
		if (GetKeyboardButtonDown(KeyboardButton.Three))
			SetBackgroundColor(Color.Blue);
	}
}