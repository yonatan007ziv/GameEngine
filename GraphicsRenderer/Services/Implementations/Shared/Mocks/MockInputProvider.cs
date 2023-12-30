using GraphicsRenderer.Components.Shared.Input;
using GraphicsRenderer.Services.Interfaces.InputProviders;

namespace GraphicsRenderer.Services.Implementations.Shared.Mocks;

internal class MockInputProvider : IInputProvider
{
	public bool IsKeyDown(KeyboardButton keyboardButton)
		=> true;
}