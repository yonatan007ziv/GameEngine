using GraphicsRenderer.Components.Shared.Input;
using GraphicsRenderer.Services.Interfaces.InputProviders;

namespace GraphicsRenderer.Services.Implementations.OpenGL.Input;

internal class OpenGLInputProvider : IInputProvider
{
	private readonly IMouseInputProvider mouseInputProvider;
	private readonly IKeyboardInputProvider keyboardInputProvider;

	public OpenGLInputProvider(IMouseInputProvider mouseInputProvider, IKeyboardInputProvider keyboardInputProvider)
	{
		this.mouseInputProvider = mouseInputProvider;
		this.keyboardInputProvider = keyboardInputProvider;
	}

	public bool IsKeyDown(KeyboardButton keyboardButton) => keyboardInputProvider.IsKeyDown(keyboardButton);
}