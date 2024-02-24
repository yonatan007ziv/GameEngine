using GameEngine.Components.Objects.Scriptable;
using GameEngine.Core.Components.Input.Buttons;
using System.Numerics;

namespace GameEngine.Components.UIComponents;

public class UITextBox : ScriptableUIObject
{
	public string Text { get; set; } = "";
	public float FontSize { get; set; } = 10;

	protected virtual void OnSelected() { }
	protected virtual void OnDeselected() { }

	public override void Update(float deltaTime)
	{
		if (GetMouseButtonDown(MouseButton.Mouse0))
		{
			// Inside the object
			Vector2 mousePos = GetNormalizedMousePosition();

			bool insideX = mousePos.X <= Transform.Position.X + (Transform.Scale.X / 2) && mousePos.X >= Transform.Position.X - (Transform.Scale.X / 2);
			bool insideY = mousePos.Y <= Transform.Position.Y + (Transform.Scale.Y / 2) && mousePos.Y >= Transform.Position.Y - (Transform.Scale.Y / 2);

			if (insideX && insideY)
			{
				OnSelected();
			}

			// Check if on current ui element or not
		}
	}
}