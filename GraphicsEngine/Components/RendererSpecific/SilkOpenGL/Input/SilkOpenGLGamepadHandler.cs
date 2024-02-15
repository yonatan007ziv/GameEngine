using GameEngine.Core.Components.Input.Buttons;
using GameEngine.Core.Components.Input.Events;
using GraphicsEngine.Components.Interfaces;
using Silk.NET.Input;
using System.Numerics;

namespace GraphicsEngine.Components.RendererSpecific.SilkOpenGL.Input;

internal class SilkOpenGLGamepadHandler : IInputHandler
{
	public string Name { get; }

	private readonly IGamepad gamepad;
	private readonly Action<GamepadEventData>? GamepadEvent;
	private readonly GamepadEventData gamepadEventData;

	public SilkOpenGLGamepadHandler(IGamepad gamepad, Action<GamepadEventData>? GamepadEvent)
	{
		Name = gamepad.Name;

		this.gamepad = gamepad;
		this.GamepadEvent = GamepadEvent;
		gamepadEventData = new GamepadEventData();

		gamepad.Deadzone = new Deadzone(0.1f, DeadzoneMethod.Traditional);
		gamepad.ButtonUp += GamepadButtonUp;
		gamepad.ButtonDown += GamepadButtonDown;
		gamepad.TriggerMoved += GamepadTriggerMoved;
		gamepad.ThumbstickMoved += GamepadThumbstickMoved;
	}

	#region Gamepad actions
	private void GamepadButtonUp(IGamepad gamepad, Button button)
	{
		gamepadEventData.Set(GamepadEventType.GamepadButton, Vector2.Zero, 0, GamepadAxis.None, button.Name.Translate(), false);
		GamepadEvent?.Invoke(gamepadEventData);
	}
	private void GamepadButtonDown(IGamepad gamepad, Button button)
	{
		gamepadEventData.Set(GamepadEventType.GamepadButton, Vector2.Zero, 0, GamepadAxis.None, button.Name.Translate(), true);
		GamepadEvent?.Invoke(gamepadEventData);
	}
	private void GamepadTriggerMoved(IGamepad gamepad, Trigger trigger)
	{
		GamepadAxis movedTrigger = trigger.Index == 0 ? GamepadAxis.LeftTrigger : GamepadAxis.RightTrigger;
		gamepadEventData.Set(GamepadEventType.GamepadTrigger, Vector2.Zero, trigger.Position, movedTrigger, GamepadButton.None, false);
		GamepadEvent?.Invoke(gamepadEventData);
	}
	private void GamepadThumbstickMoved(IGamepad gamepad, Thumbstick thumbstick)
	{
		GamepadAxis movedThumbstick = thumbstick.Index == 0 ? GamepadAxis.LeftAnalogHorizontal : GamepadAxis.RightAnalogHorizontal;
		gamepadEventData.Set(GamepadEventType.GamepadAnalog, new Vector2(thumbstick.X, thumbstick.Y), 0, movedThumbstick, GamepadButton.None, false);
		GamepadEvent?.Invoke(gamepadEventData);
	}
	#endregion

	public void Disconnect()
	{
		gamepad.ButtonUp -= GamepadButtonUp;
		gamepad.ButtonDown -= GamepadButtonDown;
		gamepad.TriggerMoved -= GamepadTriggerMoved;
		gamepad.ThumbstickMoved -= GamepadThumbstickMoved;
	}
}