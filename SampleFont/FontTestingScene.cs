using GameEngine.Components;
using GameEngine.Components.UIComponents;

namespace SampleFont;

internal class FontTestingScene : Scene
{
	public FontTestingScene()
	{
		UICameras.Add((new UICamera(), new GameEngine.Core.Components.ViewPort(0.5f, 0.5f, 1, 1)));


		UITextBox box = new UITextBox("WhiteGrayBorder");
		box.Transform.Scale /= 4;
		box.Transform.Position = new System.Numerics.Vector3(0.75f, 0.5f, 0);
		UIObjects.Add(box);

		UITextBox boxB = new UITextBox("WhiteGrayBorder");
		boxB.Transform.Scale /= 4;
		boxB.Transform.Position = new System.Numerics.Vector3(-0.75f, 0.5f, 0);
		UIObjects.Add(boxB);

		TestButton button = new TestButton();
		button.Transform.Scale /= 4;
		UIObjects.Add(button);
	}
}