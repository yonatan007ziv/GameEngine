using GameEngine.Core.IPC.Input;

namespace GraphicsEngine.Services.Interfaces.InputProviders;

public interface IKeyboardInputProvider
{
	bool IsKeyDown(KeyboardButton keyboardButton);
}