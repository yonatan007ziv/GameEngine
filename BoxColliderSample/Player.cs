using GameEngine.Components.ScriptableObjects;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Buttons;
using GameEngine.Extensions;
using System.Numerics;

namespace BoxColliderSample;

internal class Player : ScriptableWorldObject
{
	private const float gravityMagnitude = 10;
	private const float jumpSpeed = 50;
	private const float movementSpeed = 25;

	public readonly PlayerCameraController camera;
	private bool grounded;
	private bool addedGravity, removedGravity;
	private Vector3 gravity;

	public Player()
	{
		Tag = "Player";
		camera = new PlayerCameraController(this);
		BoxCollider = new BoxCollider(false, new Vector3(-1, -1, -1), new Vector3(1, 1, 1));

		// Gravity
		gravity = new Vector3(0, -gravityMagnitude, 0);
		Forces.Add(gravity);

		Children.Add(camera);
	}

	public override void Update(float deltaTime)
	{
		grounded = TouchingColliderTag("Ground");

		if (grounded && !removedGravity)
		{
			if (addedGravity)
				Forces.Remove(gravity);
			Velocity = new Vector3(Velocity.X, 0, Velocity.Z);

			removedGravity = true;
			addedGravity = false;
		}

		if (!grounded && !addedGravity)
		{
			if (removedGravity)
				Forces.Add(gravity);

			removedGravity = false;
			addedGravity = true;
		}

		if (GetKeyboardButtonDown(KeyboardButton.R))
			Transform.Rotation = Vector3.Zero;

		Vector3 movementVector = movementSpeed * (GetAxis("XMovement") * Transform.LocalRight + GetAxis("YMovement") * Transform.LocalRight.RotateVectorByAxis(Transform.GlobalUp, -90));
		movementVector = movementVector.ClampMagnitude(movementSpeed);
		Transform.Position += movementVector * deltaTime;

		if (GetButtonDown("Jump") && grounded)
			Velocity += Vector3.UnitY * jumpSpeed;

		if (GetButtonDown("Escape"))
			MouseLocked = !MouseLocked;
	}
}