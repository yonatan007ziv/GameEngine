using GameEngine.Core.Components.Input.Events;
using GraphicsEngine.Components.Interfaces;
using Silk.NET.Input;

namespace GraphicsEngine.Components.RendererSpecific.SilkOpenGL.Input;

internal class SilkOpenGLKeyboardHandler : IInputHandler
{
	public string Name { get; }

	private readonly IKeyboard keyboard;
	private readonly Action<KeyboardEventData>? KeyboardEvent;
	private readonly KeyboardEventData keyboardEventData;

	public SilkOpenGLKeyboardHandler(IKeyboard keyboard, Action<KeyboardEventData>? KeyboardEvent)
	{
		Name = keyboard.Name;

		this.keyboard = keyboard;
		this.KeyboardEvent = KeyboardEvent;
		keyboardEventData = new KeyboardEventData();

		keyboard.KeyUp += KeyboardKeyUp;
		keyboard.KeyDown += KeyboardKeyDown;
	}

	#region Keyboard actions
	private void KeyboardKeyUp(IKeyboard keyboard, Key key, int arg3)
	{
		keyboardEventData.Set(key.Translate(), false);
		KeyboardEvent?.Invoke(keyboardEventData);
	}
	private void KeyboardKeyDown(IKeyboard keyboard, Key key, int arg3)
	{
		keyboardEventData.Set(key.Translate(), true);
		KeyboardEvent?.Invoke(keyboardEventData);
	}
	#endregion

	public void Disconnect()
	{
		keyboard.KeyUp -= KeyboardKeyUp;
		keyboard.KeyDown -= KeyboardKeyDown;
	}
}