using GameEngine.Core.Components;
using GameEngine.Core.IPC.Input;
using GraphicsEngine.Services.Interfaces.InputProviders;
using System.Numerics;

namespace GraphicsEngine.Components.Shared;

internal class FreeMovementController
{
	private readonly Transform transform;
	private readonly IInputProvider inputProvider;

	public FreeMovementController(Transform parent, IInputProvider inputProvider)
	{
		transform = parent;
		this.inputProvider = inputProvider;
	}

	public void UpdateInput(float deltaTime)
	{
		float movementSpeed = inputProvider.IsKeyDown(KeyboardButton.LShift) ? 25 : 10;
		Vector3 finalVelocityVector = new Vector3();

		Vector3 wasdDirectionVector = new Vector3();
		if (inputProvider.IsKeyDown(KeyboardButton.W))
			wasdDirectionVector += transform.LocalFront;
		if (inputProvider.IsKeyDown(KeyboardButton.A))
			wasdDirectionVector -= transform.LocalRight;
		if (inputProvider.IsKeyDown(KeyboardButton.S))
			wasdDirectionVector -= transform.LocalFront;
		if (inputProvider.IsKeyDown(KeyboardButton.D))
			wasdDirectionVector += transform.LocalRight;

		Vector3 upDownDirectionVector = new Vector3();
		if (inputProvider.IsKeyDown(KeyboardButton.E))
			upDownDirectionVector += Transform.GlobalUp;
		if (inputProvider.IsKeyDown(KeyboardButton.Q))
			upDownDirectionVector -= Transform.GlobalUp;

		finalVelocityVector += (wasdDirectionVector == Vector3.Zero ? Vector3.Zero : Vector3.Normalize(wasdDirectionVector));
		finalVelocityVector += (upDownDirectionVector == Vector3.Zero ? Vector3.Zero : Vector3.Normalize(upDownDirectionVector));

		transform.Position += finalVelocityVector * movementSpeed * deltaTime;
	}
}