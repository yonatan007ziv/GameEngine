using GameEngine.Core.Components.Input.Buttons;

namespace GraphicsEngine.Components.RendererSpecific.OpenTK;

internal static class OpenTKInputExtensions
{
	public static MouseButton Translate(this global::OpenTK.Windowing.GraphicsLibraryFramework.MouseButton btn)
	{
		switch (btn)
		{
			case global::OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Left:
				return MouseButton.Mouse0;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Right:
				return MouseButton.Mouse1;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Middle:
				return MouseButton.Mouse2;
		}
		return MouseButton.None;
	}

	public static KeyboardButton Translate(this global::OpenTK.Windowing.GraphicsLibraryFramework.Keys btn)
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
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.Right:
				return KeyboardButton.RightArrow;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.Left:
				return KeyboardButton.LeftArrow;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.Up:
				return KeyboardButton.UpArrow;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.Down:
				return KeyboardButton.DownArrow;

			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.LeftShift:
				return KeyboardButton.LShift;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.LeftControl:
				return KeyboardButton.LCtrl;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.LeftAlt:
				return KeyboardButton.LAlt;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.RightShift:
				return KeyboardButton.RShift;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.RightControl:
				return KeyboardButton.RCtrl;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.RightAlt:
				return KeyboardButton.RAlt;

			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.Space:
				return KeyboardButton.Space;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.Period:
				return KeyboardButton.Period;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.Comma:
				return KeyboardButton.Comma;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.Enter:
				return KeyboardButton.Enter;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.Tab:
				return KeyboardButton.Tab;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape:
				return KeyboardButton.Escape;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.CapsLock:
				return KeyboardButton.CapsLock;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.GraveAccent:
				return KeyboardButton.Backtick;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.Semicolon:
				return KeyboardButton.Semicolon;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.Equal:
				return KeyboardButton.Equal;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.Minus:
				return KeyboardButton.Minus;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.Apostrophe:
				return KeyboardButton.Apostrophe;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.Slash:
				return KeyboardButton.Slash;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.Backslash:
				return KeyboardButton.Backslash;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.LeftBracket:
				return KeyboardButton.LeftSquareBracket;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.RightBracket:
				return KeyboardButton.RightSquareBracket;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.Backspace:
				return KeyboardButton.Backspace;
			case global::OpenTK.Windowing.GraphicsLibraryFramework.Keys.Delete:
				return KeyboardButton.Delete;
		}

		return KeyboardButton.None;
	}
}