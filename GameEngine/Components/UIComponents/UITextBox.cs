namespace GameEngine.Components.UIComponents;

public class UITextBox : UIButton
{
	private bool editingEnabled;

	public UITextBox(string materialName)
		: base(materialName)
	{
		// Enable textbox editing
		OnDragClicked += () => editingEnabled = true;

		// Disable textbox editing
		OnDeselected += () => editingEnabled = false;
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);

		if (editingEnabled)
		{
			string prev = TextData.Text;
			TextData.Text = CaptureKeyboardInput(TextData.Text);
			if (prev != TextData.Text)
				Console.WriteLine(TextData.Text);
		}
	}
}