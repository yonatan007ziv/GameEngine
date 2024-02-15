using GameEngine.Core.Components.Input.Buttons;

namespace GameEngine.Core.Components.Input.Events;

public class KeyboardEventData
{
	public KeyboardButton KeyboardButton { get; private set; }
	public bool Pressed { get; private set; }

	public void Set(KeyboardButton keyboardButton, bool pressed)
	{
		KeyboardButton = keyboardButton;
		Pressed = pressed;
	}
}