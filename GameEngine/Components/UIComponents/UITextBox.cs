namespace GameEngine.Components.UIComponents;

public class UITextBox : UIButton
{
	private bool editingEnabled; // The TextBox is in edit mode

	// Appropriately use the button's events to tell when the editing should be enabled
	public UITextBox()
	{
		// Enable textbox editing
		OnDragClicked += () => editingEnabled = true;

		// Disable textbox editing
		OnDeselected += () => editingEnabled = false;
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);

		// Change text based on keyboard input
		if (editingEnabled)
			TextData.Text = CaptureKeyboardInput(TextData.Text);
	}
}