using GameEngine.Components.UIComponents;
using GameEngine.Core.Components.Input.Buttons;

namespace SampleFont;

internal class TestButton : UIButton
{
	public TestButton()
	{

	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);
		if (GetKeyboardButtonDown(KeyboardButton.A))
			Visible = !Visible;
	}
}