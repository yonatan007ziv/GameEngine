using GameEngine.Components.Objects.Scriptable;
using GameEngine.Core.Components;
using GameEngine.Extensions;
using System.Numerics;

namespace BoxColliderSample;

internal class Player : ScriptableWorldObject
{
	private const float gravityMagnitude = 20;
	private const float jumpSpeed = 20;
	private const float movementSpeed = 25;

	public readonly PlayerCameraController camera;

	public Player()
	{
		camera = new PlayerCameraController(this);
		BoxCollider = new BoxColliderData(false, new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
	}

	public override void Update(float deltaTime)
	{
		if (TouchingColliderTag("Ground"))
		{
			Forces.Clear();
			// Temp, need to have a Velocity property instead of ImpulseVelocities and set the Y to 0
			ImpulseVelocities.Clear();
		}
		else if (Forces.Count == 0)
			Forces.Add(new Vector3(0, -gravityMagnitude, 0));

		Vector3 movementVector = movementSpeed * (GetAxis("XMovement") * Transform.LocalRight + GetAxis("YMovement") * Transform.LocalRight.RotateVectorByAxis(Transform.GlobalUp, -90));
		movementVector = movementVector.ClampMagnitude(movementSpeed);
		Transform.Position += movementVector * deltaTime;

		if (GetButtonDown("Jump"))
			ImpulseVelocities.Add(new Vector3(0, jumpSpeed, 0));

		if (GetButtonDown("Escape"))
			MouseLocked = !MouseLocked;
	}
}