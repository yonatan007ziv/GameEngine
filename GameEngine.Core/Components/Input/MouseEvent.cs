namespace GameEngine.Core.Components.Input;

public struct MouseEvent
{
	public MouseButton MouseButton { get; private set; }
	public bool Pressed { get; private set; }

	public void Set(MouseButton mouseButton, bool pressed)
	{
		MouseButton = mouseButton;
		Pressed = pressed;
	}
}