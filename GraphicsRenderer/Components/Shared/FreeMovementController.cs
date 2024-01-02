using GraphicsRenderer.Components.Shared.Input;
using GraphicsRenderer.Services.Interfaces.InputProviders;
using System.Numerics;

namespace GraphicsRenderer.Components.Shared;

public class FreeMovementController
{
	private readonly Transform parentTransform;
	private readonly IInputProvider inputProvider;

	public FreeMovementController(GameObject parent, IInputProvider inputProvider)
	{
		parentTransform = parent.Transform;
		this.inputProvider = inputProvider;
	}

	public void UpdateInput(float deltaTime)
	{
		float movementSpeed = inputProvider.IsKeyDown(KeyboardButton.LSHIFT) ? 25 : 10;
		Vector3 finalVelocityVector = new Vector3();

		Vector3 wasdDirectionVector = new Vector3();
        if (inputProvider.IsKeyDown(KeyboardButton.W))
			wasdDirectionVector += parentTransform.LocalFront;
		if (inputProvider.IsKeyDown(KeyboardButton.A))
			wasdDirectionVector -= parentTransform.LocalRight;
		if (inputProvider.IsKeyDown(KeyboardButton.S))
			wasdDirectionVector -= parentTransform.LocalFront;
		if (inputProvider.IsKeyDown(KeyboardButton.D))
			wasdDirectionVector += parentTransform.LocalRight;

		Vector3 upDownDirectionVector = new Vector3();
		if (inputProvider.IsKeyDown(KeyboardButton.E))
			upDownDirectionVector += Transform.GlobalUp;
		if (inputProvider.IsKeyDown(KeyboardButton.Q))
			upDownDirectionVector -= Transform.GlobalUp;

		finalVelocityVector += (wasdDirectionVector == Vector3.Zero ? Vector3.Zero : Vector3.Normalize(wasdDirectionVector));
		finalVelocityVector += (upDownDirectionVector == Vector3.Zero ? Vector3.Zero : Vector3.Normalize(upDownDirectionVector));

		parentTransform.Position += finalVelocityVector * movementSpeed * deltaTime;
	}
}