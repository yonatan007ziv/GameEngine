using GameEngine.Components.ScriptableObjects;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Buttons;
using System.Numerics;

namespace GameEngine.Components.UIComponents;

public class UIButton : ScriptableUIObject
{
	private bool _onEnterCalled = true, _onExitCalled = true, _clicked = true, _released = true;

	protected virtual void OnFullClicked() { }
	protected virtual void OnDragClicked() { }
	protected virtual void OnDeselected() { }
	protected virtual void OnReleased() { }
	protected virtual void OnEnter() { }
	protected virtual void OnExit() { }

	public UIButton(string materialName)
	{
		Meshes.Add(new MeshData("UIPlane.obj", $"{materialName}.mat"));
	}

	public override void Update(float deltaTime)
	{
		if (MouseLocked)
		{
			if (_clicked && !_released)
			{
				OnReleased();
				_released = true;
			}

			if (_onEnterCalled && !_onExitCalled)
			{
				OnExit();
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
				OnEnter();
				_onEnterCalled = true;
			}
			_onExitCalled = false;
		}
		else if (!(insideX && insideY))
		{
			if (!_onExitCalled)
			{
				OnExit();
				_onExitCalled = true;
			}
			_onEnterCalled = false;
		}

		if (insideX && insideY && GetMouseButtonDown(MouseButton.Mouse0))
		{
			OnDragClicked();
			_clicked = true;
			_released = false;
		}
		else if (!GetMouseButtonPressed(MouseButton.Mouse0))
		{
			// Mouse was clicked last time checked and not now
			if (!_released && _clicked)
			{
				if (insideX && insideY)
					OnFullClicked();

				OnReleased();
				_released = true;
				_clicked = false;
			}
		}


		if (!(insideX && insideY) && GetMouseButtonDown(MouseButton.Mouse0))
			OnDeselected();
	}
}