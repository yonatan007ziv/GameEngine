using GameEngine.Components.ScriptableObjects;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Buttons;
using GameEngine.Extensions;
using System.Numerics;

namespace BoxColliderSample;

internal class Player : ScriptableWorldObject
{
	private const float gravityMagnitude = 10;
	private const float jumpSpeed = 20;
	private const float movementSpeed = 25;

	public readonly PlayerCameraController camera;
	private bool canJump;

	public Player()
	{
		camera = new PlayerCameraController(this);
		BoxCollider = new BoxCollider(false, new Vector3(-1, -1, -1), new Vector3(1, 1, 1));

		// Gravity
		Forces.Add(new Vector3(0, -gravityMagnitude, 0));
	}

	public override void Update(float deltaTime)
	{
		if (TouchingColliderTag("Ground"))
		{
			Velocity = new Vector3(Velocity.X, 0, Velocity.Z);
			canJump = true;
		}
		else
			canJump = false;

		Vector3 movementVector = movementSpeed * (GetAxis("XMovement") * Transform.LocalRight + GetAxis("YMovement") * Transform.LocalRight.RotateVectorByAxis(Transform.GlobalUp, -90));
		movementVector = movementVector.ClampMagnitude(movementSpeed);
		Transform.Position += movementVector * deltaTime;

		if (GetButtonDown("Jump") && canJump)
			Velocity += Vector3.UnitY * jumpSpeed;

		if (GetButtonDown("Escape"))
			MouseLocked = !MouseLocked;

		if (GetKeyboardButtonDown(KeyboardButton.Enter))
			GC.Collect();
	}
}