using GameEngine.Core.Components.Input.Buttons;
using System.Numerics;

namespace GameEngine.Core.Components.Input.Events;

public class MouseEventData
{
    public MouseEventType MouseEventType { get; private set; }

    public Vector2 MousePosition { get; private set; }
    public MouseButton MouseButton { get; private set; }
    public bool Pressed { get; private set; }

    public void Set(MouseEventType mouseEventType, Vector2 mousePos, MouseButton mouseButton, bool pressed)
    {
        MouseEventType = mouseEventType;
        MousePosition = mousePos;
        MouseButton = mouseButton;
        Pressed = pressed;
    }
}

public enum MouseEventType
{
    None,
    MouseMove,
    MouseButton
}