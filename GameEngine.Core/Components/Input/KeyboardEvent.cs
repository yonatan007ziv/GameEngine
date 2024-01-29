namespace GameEngine.Core.Components.Input;

public struct KeyboardEvent
{
	public KeyboardButton KeyboardButton { get; private set; }
	public bool Pressed { get; private set; }

	public void Set(KeyboardButton keyboardButton, bool pressed)
	{
		KeyboardButton = keyboardButton;
		Pressed = pressed;
	}
}