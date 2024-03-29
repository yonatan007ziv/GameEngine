using GameEngine.Components;
using GameEngine.Components.UIComponents;

namespace SampleFont;

internal class FontTestingScene : Scene
{
	public FontTestingScene()
	{
		UICameras.Add((new UICamera(), new GameEngine.Core.Components.ViewPort(0.5f, 0.5f, 1, 1)));

		int boxCount = 2;
		for (int i = 0; i < boxCount; i++)
		{
			UITextBox box = new UITextBox("WhiteGrayBorder.mat");
			box.Transform.Scale = new System.Numerics.Vector3(1, 1f / boxCount, 1);
			box.Transform.Position = new System.Numerics.Vector3(0, (float)i / boxCount * 2 - 1 + 1f / boxCount, 0);
			UIObjects.Add(box);
		}

		// TestButton button = new TestButton();
		// button.Transform.Scale /= 4;
		// UIObjects.Add(button);
	}
}