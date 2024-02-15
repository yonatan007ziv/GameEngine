using GameEngine.Core.Components.Input.Buttons;

namespace GameEngine.Components.Interfaces;

internal interface IInputContext
{
	float GetAxis(string axis);
	float GetAxisRaw(string axis);

	bool GetButtonPressed(string buttonName);
	bool GetButtonDown(string buttonName);

	bool GetMouseButtonPressed(MouseButton mouseButton);
	bool GetMouseButtonDown(MouseButton mouseButton);

	bool GetKeyboardButtonPressed(KeyboardButton keyboardButton);
	bool GetKeyboardButtonDown(KeyboardButton keyboardButton);

	bool GetJoystickButtonPressed(GamepadButton joystickButton);
	bool GetJoystickButtonDown(GamepadButton joystickButton);
}