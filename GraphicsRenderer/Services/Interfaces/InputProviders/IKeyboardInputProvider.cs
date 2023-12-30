using GraphicsRenderer.Components.Shared.Input;

namespace GraphicsRenderer.Services.Interfaces.InputProviders;

internal interface IKeyboardInputProvider
{
	bool IsKeyDown(KeyboardButton keyboardButton);
}