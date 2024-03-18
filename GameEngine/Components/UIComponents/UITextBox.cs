namespace GameEngine.Components.UIComponents;

public class UITextBox : UIButton
{
	private bool editingEnabled;

	// Enable textbox editing
	protected override void OnDragClicked()
	{
		editingEnabled = true;
	}

	// Enable textbox editing
	protected override void OnDeselected()
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

		if (editingEnabled)
		{
			string prev = TextData.Text;
			TextData.Text = CaptureKeyboardInput(TextData.Text);
			if (prev != TextData.Text)
				Console.WriteLine(TextData.Text);
		}
	}
}