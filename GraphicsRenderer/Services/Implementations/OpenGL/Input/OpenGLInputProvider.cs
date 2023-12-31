using GraphicsRenderer.Components.Shared.Input;
using GraphicsRenderer.Services.Interfaces.InputProviders;
using System.Numerics;

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

	public Vector2 MousePosition => mouseInputProvider.MousePosition;
	public bool IsKeyDown(KeyboardButton keyboardButton) => keyboardInputProvider.IsKeyDown(keyboardButton);
}