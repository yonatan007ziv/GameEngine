using GameEngine.Core.Components.Input.Buttons;

namespace GraphicsEngine.Components.RendererSpecific.SilkOpenGL;

internal static class SilkOpenGLInputExtensions
{
	public static MouseButton Translate(this Silk.NET.Input.MouseButton btn)
	{
		switch (btn)
		{
			case Silk.NET.Input.MouseButton.Left:
				return MouseButton.Mouse0;
			case Silk.NET.Input.MouseButton.Right:
				return MouseButton.Mouse1;
			case Silk.NET.Input.MouseButton.Middle:
				return MouseButton.Mouse2;
		}
		return MouseButton.None;
	}

	public static KeyboardButton Translate(this Silk.NET.Input.Key btn)
	{
		int val = (int)btn;

		// 0 - 9
		if (48 <= val && val <= 57)
			return (KeyboardButton)val;

		// A - Z
		if (65 <= val && val <= 90)
			return (KeyboardButton)val;

		// F1 - F12
		if (290 <= val && val <= 301)
			return (KeyboardButton)(val - 189);

		switch (btn)
		{
			case Silk.NET.Input.Key.Right:
				return KeyboardButton.RightArrow;
			case Silk.NET.Input.Key.Left:
				return KeyboardButton.LeftArrow;
			case Silk.NET.Input.Key.Up:
				return KeyboardButton.UpArrow;
			case Silk.NET.Input.Key.Down:
				return KeyboardButton.DownArrow;

			case Silk.NET.Input.Key.ShiftLeft:
				return KeyboardButton.LShift;
			case Silk.NET.Input.Key.ControlLeft:
				return KeyboardButton.LCtrl;
			case Silk.NET.Input.Key.AltLeft:
				return KeyboardButton.LAlt;
			case Silk.NET.Input.Key.ShiftRight:
				return KeyboardButton.RShift;
			case Silk.NET.Input.Key.ControlRight:
				return KeyboardButton.RCtrl;
			case Silk.NET.Input.Key.AltRight:
				return KeyboardButton.RAlt;

			case Silk.NET.Input.Key.Space:
				return KeyboardButton.Space;
			case Silk.NET.Input.Key.Enter:
				return KeyboardButton.Enter;
			case Silk.NET.Input.Key.Tab:
				return KeyboardButton.Tab;
			case Silk.NET.Input.Key.Escape:
				return KeyboardButton.Escape;
			case Silk.NET.Input.Key.CapsLock:
				return KeyboardButton.CapsLock;
			case Silk.NET.Input.Key.GraveAccent:
				return KeyboardButton.Backtick;
			case Silk.NET.Input.Key.Semicolon:
				return KeyboardButton.Semicolon;
			case Silk.NET.Input.Key.Equal:
				return KeyboardButton.Equal;
			case Silk.NET.Input.Key.Minus:
				return KeyboardButton.Minus;
			case Silk.NET.Input.Key.Apostrophe:
				return KeyboardButton.Apostrophe;
			case Silk.NET.Input.Key.Slash:
				return KeyboardButton.Slash;
			case Silk.NET.Input.Key.BackSlash:
				return KeyboardButton.Backslash;
			case Silk.NET.Input.Key.Delete:
				return KeyboardButton.Delete;
		}

		return KeyboardButton.None;
	}

	public static GamepadButton Translate(this Silk.NET.Input.ButtonName btn)
	{
		switch (btn)
		{
			case Silk.NET.Input.ButtonName.Unknown:
				break;
			case Silk.NET.Input.ButtonName.A:
				return GamepadButton.Cross;
			case Silk.NET.Input.ButtonName.B:
				return GamepadButton.Circle;
			case Silk.NET.Input.ButtonName.X:
				return GamepadButton.Square;
			case Silk.NET.Input.ButtonName.Y:
				return GamepadButton.Triangle;
			case Silk.NET.Input.ButtonName.LeftBumper:
				return GamepadButton.L1;
			case Silk.NET.Input.ButtonName.RightBumper:
				return GamepadButton.R1;
			case Silk.NET.Input.ButtonName.Back:
				return GamepadButton.Select;
			case Silk.NET.Input.ButtonName.Start:
				return GamepadButton.Start;
			case Silk.NET.Input.ButtonName.Home:
				return GamepadButton.Home;
			case Silk.NET.Input.ButtonName.LeftStick:
				return GamepadButton.L3;
			case Silk.NET.Input.ButtonName.RightStick:
				return GamepadButton.R3;
			case Silk.NET.Input.ButtonName.DPadUp:
				return GamepadButton.UpDPad;
			case Silk.NET.Input.ButtonName.DPadRight:
				return GamepadButton.RightDPad;
			case Silk.NET.Input.ButtonName.DPadDown:
				return GamepadButton.DownDPad;
			case Silk.NET.Input.ButtonName.DPadLeft:
				return GamepadButton.LeftDPad;
		}
		return GamepadButton.None;
	}
}