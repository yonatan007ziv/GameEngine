using GameEngine.Core.Components.Input.Buttons;
using GameEngine.Core.Components.Input.Events;
using System.Numerics;

namespace GameEngine.Core.API;

public interface IInputEngine
{
	bool LogInputs { get; set; } // Whether to log inputs or not

	void InputTickPass(); // Passes an input tick
	Vector2 GetMousePos(); // Gets the mouse position

	float GetAxis(string axis); // Gets the registered axis's name value
	int GetAxisRaw(string axis); // Gets the registered axis's name value raw, -1 0 or 1

	bool GetButtonPressed(string buttonName); // Gets whether a registered button name is pressed
	bool GetButtonDown(string buttonName); // Gets whether a registered button name was clicked

	bool GetMouseButtonPressed(MouseButton mouseButton); // Gets whether the mouse button is pressed
	bool GetMouseButtonDown(MouseButton mouseButton); // Gets whether the mouse button was clicked

	string CaptureKeyboardInput(string input); // Returns a new string modified with the keyboard's input from the given string
	bool GetKeyboardButtonPressed(KeyboardButton keyboardButton); // Gets whether the keyboard button is pressed
	bool GetKeyboardButtonDown(KeyboardButton keyboardButton); // Gets whether the keyboard button was clicked

	bool GetGamepadButtonPressed(GamepadButton gamepadButton); // Gets whether the gamepad button is pressed
	bool GetGamepadButtonDown(GamepadButton gamepadButton); // Gets whether the gamepad button was clicked

	// Processes a mouse event
	void OnMouseEvent(MouseEventData mouseEvent);
	// Processes a keyboard event
	void OnKeyboardEvent(KeyboardEventData keyboardEvent);
	// Processes a gamepad event
	void OnGamepadEvent(GamepadEventData gamepadEvent);

	#region Input mappers
	void MapMouseButton(string buttonName, MouseButton mouseButton);
	void MapKeyboardButton(string buttonName, KeyboardButton keyboardButton);
	void MapGamepadButton(string buttonName, GamepadButton gamepadButton);

	void MapMouseAxis(string axis, MouseAxis movementType, float multiplier = 1, float offset = 0);
	void MapKeyboardAxis(string axis, KeyboardButton positive, KeyboardButton negative, float multiplier = 1, float offset = 0);
	void MapGamepadAxis(string axis, GamepadAxis analog, float multiplier = 1, float offset = 0);
	#endregion
}