using GameEngine.Components.Objects;
using GameEngine.Components.Objects.Scriptable;
using GameEngine.Core.Components;
using GameEngine.Extensions;
using System.Numerics;

namespace Sample1.Components;

internal class MovementController : ScriptableWorldComponent
{
	private const float jumpSpeed = 20;
	private const float movementSpeed = 25;

	private readonly PlayerMovementControls movementControls;

	public MovementController(WorldObject parent, PlayerMovementControls movementControls)
		: base(parent)
	{
		this.movementControls = movementControls;
	}

	public override void Update(float deltaTime)
	{
		Vector3 movementVector = movementSpeed * (-GetAxis(movementControls.AxesSet.Horizontal) * Transform.LocalRight + GetAxis(movementControls.AxesSet.Vertical) * Transform.LocalFront);
		movementVector = movementVector.ClampMagnitude(movementSpeed);
		Transform.Position += movementVector * deltaTime;

		if (GetButtonDown(movementControls.Jump))
			ImpulseVelocities.Add(new Vector3(0, jumpSpeed, 0));

		if (GetButtonDown(movementControls.Pause))
			MouseLocked = !MouseLocked;
	}
}