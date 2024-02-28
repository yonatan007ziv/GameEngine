namespace GameEngine.Components.UIComponents;

public class UITextBox : UIButton
{
	public string Text { get; set; } = "";
	public float FontSize { get; set; } = 10;

	private bool editingEnabled;

	// Enable textbox editing
	protected override void OnDragClicked()
	{
		editingEnabled = true;
	}

	// Enable textbox editing
	private void OnDeselected()
	{
		editingEnabled = false;
	}

	public UITextBox(string materialName)
		: base(materialName)
	{

	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);

		// Check if deselected
		// OnDeselected()

		if (editingEnabled)
		{
			Text += GetRecentKeyboardInput();
		}
	}
}