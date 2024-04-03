using GameEngine.Components.ScriptableObjects;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Buttons;
using System.Numerics;

namespace GameEngine.Components.UIComponents;

public class UIButton : ScriptableUIObject
{
	private bool _onEnterCalled = true, _onExitCalled = true, _clicked = true, _released = true;

	private bool _enabled = true;
	public bool Enabled
	{
		get => _enabled;
		set
		{
			_enabled = value;

			string chosenMaterial = _enabled ? Material : DisabledMaterial;
			Meshes[0] = new MeshData("UIRect.obj", chosenMaterial);
		}
	}

	public string Material { get; }
	public string DisabledMaterial { get; set; } = "Default.mat";

	public event Action? OnFullClicked;
	public event Action? OnDragClicked;
	public event Action? OnDeselected;
	public event Action? OnReleased;
	public event Action? OnEnter;
	public event Action? OnExit;

	private void FullClicked() => OnFullClicked?.Invoke();
	private void DragClicked() => OnDragClicked?.Invoke();
	private void Deselected() => OnDeselected?.Invoke();
	private void Released() => OnReleased?.Invoke();
	private void Enter() => OnEnter?.Invoke();
	private void Exit() => OnExit?.Invoke();

	public UIButton()
	{
		Meshes.Add(new MeshData("UIRect.obj", "Default.mat"));
		Material = "Default.mat";
	}

	public override void Update(float deltaTime)
	{
		// Disabled or not visible
		if (!Enabled || !Visible || !(Parent?.Visible ?? true))
			return;

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

		(Vector3 relativePosition, Vector3 relativeRotation, Vector3 relativeScale) = GetRelativeToAncestorTransform();
		bool insideX = mousePos.X <= (relativePosition.X + relativeScale.X) && mousePos.X >= (relativePosition.X - relativeScale.X);
		bool insideY = mousePos.Y <= (relativePosition.Y + relativeScale.Y) && mousePos.Y >= (relativePosition.Y - relativeScale.Y);

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