using System.Numerics;

namespace GameEngine.Core.IPC.Input;

internal class InputEvent
{
	public readonly Vector2 mousePos;
	public readonly MouseButton mouseButton;
	public readonly KeyboardButton keyboardButton;
}