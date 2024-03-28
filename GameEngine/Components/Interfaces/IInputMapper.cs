using GameEngine.Core.Components.Input.Buttons;

namespace GameEngine.Components.Interfaces;

internal interface IInputMapper
{
    void MapMouseButton(string buttonName, MouseButton mouseButton);
    void MapKeyboardButton(string buttonName, KeyboardButton keyboardButton);
    void MapGamepadButton(string buttonName, GamepadButton gamepadButton);

    void MapMouseAxis(string axis, MouseAxis movementType, float multiplier, float offset);
    void MapKeyboardAxis(string axis, KeyboardButton positive, KeyboardButton negative, float multiplier, float offset);
    void MapGamepadAxis(string axis, GamepadAxis analog, float multiplier, float offset);
}