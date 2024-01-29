using GameEngine.Core.Components.Input;

namespace GraphicsEngine.Components.OpenGL;

internal static class OpenGLInputExtensions
{
	public static MouseButton Translate(this OpenTK.Windowing.GraphicsLibraryFramework.MouseButton btn)
	{
		switch (btn)
		{
			case OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Left:
				return MouseButton.Mouse0;
			case OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Right:
				return MouseButton.Mouse1;
			case OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Middle:
				return MouseButton.Mouse2;
		}
		return MouseButton.None;
	}

	public static KeyboardButton Translate(this OpenTK.Windowing.GraphicsLibraryFramework.Keys btn)
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
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Right:
				return KeyboardButton.RightArrow;
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Left:
				return KeyboardButton.LeftArrow;
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Up:
				return KeyboardButton.UpArrow;
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Down:
				return KeyboardButton.DownArrow;

			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.LeftShift:
				return KeyboardButton.LShift;
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.LeftControl:
				return KeyboardButton.LCtrl;
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.LeftAlt:
				return KeyboardButton.LAlt;
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.RightShift:
				return KeyboardButton.RShift;
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.RightControl:
				return KeyboardButton.RCtrl;
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.RightAlt:
				return KeyboardButton.RAlt;

			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Space:
				return KeyboardButton.Space;
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Enter:
				return KeyboardButton.Enter;
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Tab:
				return KeyboardButton.Tab;
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape:
				return KeyboardButton.Escape;
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.CapsLock:
				return KeyboardButton.CapsLock;
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.GraveAccent:
				return KeyboardButton.Backtick;
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Semicolon:
				return KeyboardButton.Semicolon;
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Equal:
				return KeyboardButton.Equal;
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Minus:
				return KeyboardButton.Minus;
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Apostrophe:
				return KeyboardButton.Apostrophe;
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Slash:
				return KeyboardButton.Slash;
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Backslash:
				return KeyboardButton.Backslash;
			case OpenTK.Windowing.GraphicsLibraryFramework.Keys.Delete:
				return KeyboardButton.Delete;
		}

		return KeyboardButton.None;
	}
}