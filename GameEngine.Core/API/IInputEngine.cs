using GameEngine.Core.Components.Input;
using System.Numerics;

namespace GameEngine.Core.API;

public interface IInputEngine
{
	Vector2 GetMouseVector();
	Vector3 GetMovementVector(KeyboardButton right, KeyboardButton left, KeyboardButton up, KeyboardButton down);
	bool IsMouseButtonDown(MouseButton mouseButton);
	bool IsKeyboardButtonDown(KeyboardButton keyboardButton);

	void OnMouseButtonEvent(ref MouseEvent mouseEvent);
	void OnKeyboardButtonEvent(ref KeyboardEvent keyboardEvent);
	void OnMousePositionEvent(Vector2 vector);
}