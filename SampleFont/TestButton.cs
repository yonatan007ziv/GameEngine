using GameEngine.Components.UIComponents;

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
}