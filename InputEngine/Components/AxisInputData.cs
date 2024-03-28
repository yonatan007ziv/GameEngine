using GameEngine.Core.Components.Input.Buttons;

namespace InputEngine.Components;

internal class AxisInputData
{
    public bool IsMouseRegistered { get; set; }
    public MouseAxis MouseAxis { get; set; }
    public float MouseAxisOffset { get; set; }
    public float MouseAxisMultiplier { get; set; }

    public bool IsKeyboardRegistered { get; set; }
    public KeyboardButton KeyboardAxisPositive { get; set; }
    public KeyboardButton KeyboardAxisNegative { get; set; }
    public float KeyboardAxisOffset { get; set; }
    public float KeyboardAxisMultiplier { get; set; }

    public bool IsGamepadRegistered { get; set; }
    public GamepadAxis GamepadAxis { get; set; }
    public float GamepadAxisOffset { get; set; }
    public float GamepadAxisMultiplier { get; set; }
}