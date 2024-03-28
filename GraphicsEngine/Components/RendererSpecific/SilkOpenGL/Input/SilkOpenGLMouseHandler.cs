
using GameEngine.Core.Components.Input.Events;
using GraphicsEngine.Components.Interfaces;
using Silk.NET.Input;
using System.Numerics;

namespace GraphicsEngine.Components.RendererSpecific.SilkOpenGL.Input;

internal class SilkOpenGLMouseHandler : IInputHandler
{
    public string Name { get; }

    private readonly IMouse mouse;
    private readonly Action<MouseEventData>? MouseEvent;
    private readonly MouseEventData mouseEventData;

    public SilkOpenGLMouseHandler(IMouse mouse, Action<MouseEventData>? MouseEvent)
    {
        Name = mouse.Name;

        this.mouse = mouse;
        this.MouseEvent = MouseEvent;
        mouseEventData = new MouseEventData();

        mouse.MouseMove += MouseMove;
        mouse.MouseUp += MouseUp;
        mouse.MouseDown += MouseDown;
    }

    #region Mouse actions
    private void MouseMove(IMouse mouse, Vector2 position)
    {
        mouseEventData.Set(MouseEventType.MouseMove, position, GameEngine.Core.Components.Input.Buttons.MouseButton.None, false);
        MouseEvent?.Invoke(mouseEventData);
    }
    private void MouseUp(IMouse mouse, MouseButton button)
    {
        mouseEventData.Set(MouseEventType.MouseButton, Vector2.Zero, button.Translate(), false);
        MouseEvent?.Invoke(mouseEventData);
    }
    private void MouseDown(IMouse mouse, MouseButton button)
    {
        mouseEventData.Set(MouseEventType.MouseButton, Vector2.Zero, button.Translate(), true);
        MouseEvent?.Invoke(mouseEventData);
    }
    #endregion

    public void Disconnect()
    {
        mouse.MouseMove -= MouseMove;
        mouse.MouseUp -= MouseUp;
        mouse.MouseDown -= MouseDown;
    }
}