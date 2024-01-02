using GraphicsRenderer.Components.Shared.Input;
using GraphicsRenderer.Services.Implementations.OpenGL.Renderer;
using GraphicsRenderer.Services.Interfaces.InputProviders;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GraphicsRenderer.Services.Implementations.OpenGL.Input;

public class OpenGLKeyboardInputProvider : IKeyboardInputProvider
{
	public bool IsKeyDown(KeyboardButton button)
	{
		return OpenGLRenderer.Instance.KeyboardState.IsKeyDown(ConvertToOpenTKKey(button));
	}

	private Keys ConvertToOpenTKKey(KeyboardButton button)
	{
		int buttonValue = (int)button;

		// Check ABCD
		int buttonABCOffset = buttonValue;
		if ((int)Keys.A <= buttonABCOffset && buttonABCOffset <= (int)Keys.Z)
			return (Keys)buttonABCOffset;

		// Check F1 ... F12
		int buttonFOffset = (buttonValue + 189);
		if ((int)Keys.F1 <= buttonFOffset && buttonFOffset <= (int)Keys.F12)
			return (Keys)buttonFOffset;

		// Check 0 ... 9
		int button012Offset = buttonValue - 43;
		if ((int)Keys.D0 <= buttonFOffset && buttonFOffset <= (int)Keys.D9)
			return (Keys)button012Offset;

		switch (button)
		{
			case KeyboardButton.LSHIFT: return Keys.LeftShift;
			case KeyboardButton.LCTRL: return Keys.LeftControl;
			case KeyboardButton.LALT: return Keys.LeftAlt;
			case KeyboardButton.RSHIFT: return Keys.RightShift;
			case KeyboardButton.RCTRL: return Keys.RightControl;
			case KeyboardButton.RALT: return Keys.RightAlt;
		}

		return Keys.Unknown;
	}
}