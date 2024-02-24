using GameEngine.Components.Objects.Scriptable;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Buttons;
using System.Numerics;

namespace GameEngine.Components.UIComponents;

public class UIButton : ScriptableUIObject
{
	private bool _onEnterCalled, _onExitCalled, _clicked, _released;

	protected virtual void OnFullClicked() { }
	protected virtual void OnDragClicked() { }
	protected virtual void OnReleased() { }
	protected virtual void OnEnter() { }
	protected virtual void OnExit() { }

	public UIButton(MeshData uiMeshData)
	{
		Meshes.Add(uiMeshData);
	}

	public override void Update(float deltaTime)
	{
		Vector2 mousePos = GetNormalizedMousePosition();
		bool insideX = mousePos.X <= Transform.Position.X + Transform.Scale.X && mousePos.X >= Transform.Position.X - Transform.Scale.X;
		bool insideY = mousePos.Y <= Transform.Position.Y + Transform.Scale.Y && mousePos.Y >= Transform.Position.Y - Transform.Scale.Y;

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

		if (GetMouseButtonDown(MouseButton.Mouse0))
		{
			if (insideX && insideY)
			{
				OnDragClicked();
				_clicked = true;
				_released = false;
			}
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
	}
}