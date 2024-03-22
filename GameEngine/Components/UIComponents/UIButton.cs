using GameEngine.Components.ScriptableObjects;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Buttons;
using System.Numerics;

namespace GameEngine.Components.UIComponents;

public class UIButton : ScriptableUIObject
{
	private bool _onEnterCalled = true, _onExitCalled = true, _clicked = true, _released = true;

	public event Action? OnFullClicked;
	public event Action? OnDragClicked;
	public event Action? OnDeselected;
	public event Action? OnReleased;
	public event Action? OnEnter;
	public event Action? OnExit;

	private void FullClicked() => OnFullClicked?.Invoke();
	private void DragClicked() => OnFullClicked?.Invoke();
	private void Deselected() => OnFullClicked?.Invoke();
	private void Released() => OnFullClicked?.Invoke();
	private void Enter() => OnFullClicked?.Invoke();
	private void Exit() => OnFullClicked?.Invoke();

	public UIButton(string material)
	{
		Meshes.Add(new MeshData("UIRect.obj", material));
	}

	public override void Update(float deltaTime)
	{
		if (MouseLocked)
		{
			if (_clicked && !_released)
			{
				Released();
				_released = true;
			}

			if (_onEnterCalled && !_onExitCalled)
			{
				Exit();
				_onExitCalled = true;
			}

			return;
		}

		Vector2 mousePos = GetUIMousePosition();

		bool insideX = mousePos.X <= (Transform.Position.X + Transform.Scale.X) && mousePos.X >= (Transform.Position.X - Transform.Scale.X);
		bool insideY = mousePos.Y <= (Transform.Position.Y + Transform.Scale.Y) && mousePos.Y >= (Transform.Position.Y - Transform.Scale.Y);

		if (insideX && insideY)
		{
			if (!_onEnterCalled)
			{
				Enter();
				_onEnterCalled = true;
			}
			_onExitCalled = false;
		}
		else if (!(insideX && insideY))
		{
			if (!_onExitCalled)
			{
				Exit();
				_onExitCalled = true;
			}
			_onEnterCalled = false;
		}

		if (insideX && insideY && GetMouseButtonDown(MouseButton.Mouse0))
		{
			DragClicked();
			_clicked = true;
			_released = false;
		}
		else if (!GetMouseButtonPressed(MouseButton.Mouse0))
		{
			// Mouse was clicked last time checked and not now
			if (!_released && _clicked)
			{
				if (insideX && insideY)
					FullClicked();

				Released();
				_released = true;
				_clicked = false;
			}
		}


		if (!(insideX && insideY) && GetMouseButtonDown(MouseButton.Mouse0))
			Deselected();
	}
}