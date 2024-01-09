using GameEngine.Core.IPC.Input;

namespace GraphicsRenderer.Services.Interfaces.InputProviders;

public interface IKeyboardInputProvider
{
	bool IsKeyDown(KeyboardButton keyboardButton);
}