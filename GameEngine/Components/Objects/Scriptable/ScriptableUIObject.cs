using GameEngine.Core.Components.Input.Buttons;
using System.Drawing;

namespace GameEngine.Components.Objects.Scriptable;

public abstract class ScriptableUIObject : UIObject
{
    protected bool MouseLocked { get => Services.Implementations.GameEngine.EngineContext.MouseLocked; set => Services.Implementations.GameEngine.EngineContext.MouseLocked = value; }

    public virtual void OnHover() { }
    public virtual void OnClicked() { }

    public float GetAxis(string axis)
        => Services.Implementations.GameEngine.EngineContext.InputEngine.GetAxis(axis);
    public float GetAxisRaw(string axis)
        => Services.Implementations.GameEngine.EngineContext.InputEngine.GetAxisRaw(axis);

    public bool GetButtonPressed(string buttonName)
        => Services.Implementations.GameEngine.EngineContext.InputEngine.GetButtonPressed(buttonName);
    public bool GetButtonDown(string buttonName)
        => Services.Implementations.GameEngine.EngineContext.InputEngine.GetButtonDown(buttonName);

    public bool GetMouseButtonPressed(MouseButton mouseButton)
    => Services.Implementations.GameEngine.EngineContext.InputEngine.GetMouseButtonPressed(mouseButton);
    public bool GetMouseButtonDown(MouseButton mouseButton)
        => Services.Implementations.GameEngine.EngineContext.InputEngine.GetMouseButtonDown(mouseButton);

    public bool GetKeyboardButtonPressed(KeyboardButton keyboardButton)
    => Services.Implementations.GameEngine.EngineContext.InputEngine.GetKeyboardButtonPressed(keyboardButton);
    public bool GetKeyboardButtonDown(KeyboardButton keyboardButton)
        => Services.Implementations.GameEngine.EngineContext.InputEngine.GetKeyboardButtonDown(keyboardButton);

    public bool GetJoystickButtonPressed(GamepadButton joystickButton)
        => Services.Implementations.GameEngine.EngineContext.InputEngine.GetGamepadButtonPressed(joystickButton);
    public bool GetJoystickButtonDown(GamepadButton joystickButton)
        => Services.Implementations.GameEngine.EngineContext.InputEngine.GetGamepadButtonDown(joystickButton);

    public void SetBackgroundColor(Color color)
        => Services.Implementations.GameEngine.EngineContext.SetBackgroundColor(color);
}