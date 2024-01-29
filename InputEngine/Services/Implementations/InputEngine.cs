using GameEngine.Core.API;
using GameEngine.Core.Components.Input;
using System.Numerics;

namespace InputEngine.Services.Implementations;

internal class InputEngine : IInputEngine
{
	private readonly List<MouseButton> mouseButtons = new List<MouseButton>();
	private readonly List<KeyboardButton> keyboardButtons = new List<KeyboardButton>();
	private Vector2 mousePosition, lastMousePosition;

	public Vector2 GetMouseVector()
	{
		Vector2 res = mousePosition - lastMousePosition;
		lastMousePosition = mousePosition;
		return res;
	}

	public Vector3 GetMovementVector(KeyboardButton right, KeyboardButton left, KeyboardButton up, KeyboardButton down)
	{
		Vector3 movement = new Vector3();
		foreach (KeyboardButton keyboardButton in keyboardButtons)
			if (keyboardButton == right)
				movement += Vector3.UnitX;
			else if (keyboardButton == left)
				movement -= Vector3.UnitX;
			else if (keyboardButton == up)
				movement += Vector3.UnitZ;
			else if (keyboardButton == down)
				movement -= Vector3.UnitZ;
		return movement;
	}

	public bool IsMouseButtonDown(MouseButton mouseButton)
		=> mouseButtons.Contains(mouseButton);

	public bool IsKeyboardButtonDown(KeyboardButton keyboardButton)
		=> keyboardButtons.Contains(keyboardButton);

	public Vector2 GetMousePos()
		=> mousePosition;

	public void OnMouseButtonEvent(ref MouseEvent mouseEvent)
	{
		if (mouseEvent.Pressed)
		{
			if (!mouseButtons.Contains(mouseEvent.MouseButton))
				mouseButtons.Add(mouseEvent.MouseButton);
		}
		else
		{
			if (mouseButtons.Contains(mouseEvent.MouseButton))
				mouseButtons.Remove(mouseEvent.MouseButton);
		}
	}

	public void OnKeyboardButtonEvent(ref KeyboardEvent keyboardEvent)
	{
		if (keyboardEvent.Pressed)
		{
			if (!keyboardButtons.Contains(keyboardEvent.KeyboardButton))
				keyboardButtons.Add(keyboardEvent.KeyboardButton);
		}
		else
		{
			if (keyboardButtons.Contains(keyboardEvent.KeyboardButton))
				keyboardButtons.Remove(keyboardEvent.KeyboardButton);
		}
	}

	public void OnMousePositionEvent(Vector2 mousePosition)
	{
		lastMousePosition = this.mousePosition;
		this.mousePosition = mousePosition;
	}
}