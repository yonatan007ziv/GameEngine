using GameEngine.Components.UIComponents;
using GameEngine.Core.Components.Input.Buttons;

namespace SampleFont;

internal class TestButton : UIButton
{
	public TestButton()
	{
		OnFullClicked += AppearDisappear;
	}

	private async void AppearDisappear()
	{
		Visible = false;
		Enabled = false;
		await Task.Delay(500);
		Visible = true;
		Enabled = true;
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);
		if (GetKeyboardButtonDown(KeyboardButton.A))
			Visible = !Visible;
	}
}