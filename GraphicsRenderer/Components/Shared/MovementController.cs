using GraphicsRenderer.Components.Shared.Input;
using GraphicsRenderer.Services.Interfaces.InputProviders;

namespace GraphicsRenderer.Components.Shared;

internal class MovementController
{
	private readonly Transform parentTransform;

	public MovementController(GameObject parent)
	{
		parentTransform = parent.Transform;
	}

	public void UpdateInput(IInputProvider inputProvider, float deltaTime)
	{
		float movementSpeed = inputProvider.IsKeyDown(KeyboardButton.LSHIFT) ? 25 : 10;

		if (inputProvider.IsKeyDown(KeyboardButton.W))
			parentTransform.Position += parentTransform.Front * movementSpeed * deltaTime;

		if (inputProvider.IsKeyDown(KeyboardButton.A))
			parentTransform.Position -= parentTransform.Right * movementSpeed * deltaTime;

		if (inputProvider.IsKeyDown(KeyboardButton.S))
			parentTransform.Position -= parentTransform.Front * movementSpeed * deltaTime;

		if (inputProvider.IsKeyDown(KeyboardButton.D))
			parentTransform.Position += parentTransform.Right * movementSpeed * deltaTime;

		if (inputProvider.IsKeyDown(KeyboardButton.E))
			parentTransform.Position += parentTransform.Up * movementSpeed * deltaTime;

		if (inputProvider.IsKeyDown(KeyboardButton.Q))
			parentTransform.Position -= parentTransform.Up * movementSpeed * deltaTime;
	}
}