using GameEngine.Core.Components.Input.Buttons;

namespace InputEngine.Components;

// Defines data used for button input polling
internal class ButtonInputData
{
	public bool IsMouseRegistered { get; set; }
	public bool IsKeyboardRegistered { get; set; }
	public bool IsGamepadRegistered { get; set; }

	public MouseButton MouseButton { get; set; }
	public KeyboardButton KeyboardButton { get; set; }
	public GamepadButton GamepadButton { get; set; }
}