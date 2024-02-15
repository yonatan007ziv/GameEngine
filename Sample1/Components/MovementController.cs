using GameEngine.Components;
using GameEngine.Core.Components;

namespace Sample1.Components;

internal class MovementController : ScriptableGameObject
{
	private const float movementSpeed = 0.2f;

	private readonly PlayerMovementControls movementControls;

	public MovementController(Transform parent, PlayerMovementControls movementControls)
	{
		Transform = parent;
		this.movementControls = movementControls;

	}
	public override void Update(float deltaTime)
	{
		Transform.Position += (-GetAxis(movementControls.AxesSet.Horizontal) * Transform.LocalRight + GetAxis(movementControls.AxesSet.Vertical) * Transform.LocalFront) * movementSpeed;

		if (GetButtonDown(movementControls.Jump)) // Attach to parent velocities later I am tired
			Transform.Position += new System.Numerics.Vector3(0, 20, 0);

		if (GetButtonDown(movementControls.Pause))
			MouseLocked = !MouseLocked;
	}
}