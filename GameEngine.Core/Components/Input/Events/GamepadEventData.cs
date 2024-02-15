using GameEngine.Core.Components.Input.Buttons;
using System.Numerics;

namespace GameEngine.Core.Components.Input.Events;

public class GamepadEventData
{
	public GamepadEventType GamepadEventType { get; private set; }

	public Vector2 AnalogPosition { get; private set; }
	public float TriggerPosition { get; private set; }
	public GamepadAxis GamepadAxis { get; private set; }
	public GamepadButton GamepadButton { get; private set; }
	public bool Pressed { get; private set; }

	public void Set(GamepadEventType gamepadEventType, Vector2 analogPosition, float triggerPosition, GamepadAxis gamepadAxes, GamepadButton gamepadButton, bool pressed)
	{
		GamepadEventType = gamepadEventType;
		AnalogPosition = analogPosition;
		TriggerPosition = triggerPosition;
		GamepadAxis = gamepadAxes;
		GamepadButton = gamepadButton;
		Pressed = pressed;
	}
}

public enum GamepadEventType
{
	None,
	GamepadAnalog,
	GamepadTrigger,
	GamepadButton,
}