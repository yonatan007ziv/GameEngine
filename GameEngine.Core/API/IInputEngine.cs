using GameEngine.Core.Components.Input.Buttons;
using GameEngine.Core.Components.Input.Events;
using System.Numerics;

namespace GameEngine.Core.API;

public interface IInputEngine
{
	bool LogInputs { get; set; }

	void InputTickPass();
	Vector2 GetMousePos();

	float GetAxis(string axis);
	float GetAxisRaw(string axis);

	bool GetButtonPressed(string buttonName);
	bool GetButtonDown(string buttonName);

	bool GetMouseButtonPressed(MouseButton mouseButton);
	bool GetMouseButtonDown(MouseButton mouseButton);

	bool GetKeyboardButtonPressed(KeyboardButton keyboardButton);
	bool GetKeyboardButtonDown(KeyboardButton keyboardButton);

	bool GetGamepadButtonPressed(GamepadButton gamepadButton);
	bool GetGamepadButtonDown(GamepadButton gamepadButton);

	void OnMouseEvent(MouseEventData mouseEvent);
	void OnKeyboardEvent(KeyboardEventData keyboardEvent);
	void OnGamepadEvent(GamepadEventData gamepadEvent);

	// Input mapper
	void MapMouseButton(string buttonName, MouseButton mouseButton);
	void MapKeyboardButton(string buttonName, KeyboardButton keyboardButton);
	void MapGamepadButton(string buttonName, GamepadButton gamepadButton);

	void MapMouseAxis(string axis, MouseAxis movementType, float multiplier, float offset);
	void MapKeyboardAxis(string axis, KeyboardButton positive, KeyboardButton negative, float multiplier, float offset);
	void MapGamepadAxis(string axis, GamepadAxis analog, float multiplier, float offset);
}