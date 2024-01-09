using GameEngine.Core.IPC.Input;
using GraphicsEngine.Services.Interfaces.InputProviders;
using System.Numerics;

namespace GraphicsEngine.Services.Implementations.OpenGL.Input;

public class OpenGLInputProvider : IInputProvider
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