using GameEngine.Components;
using GameEngine.Components.Objects;
using GameEngine.Components.UIComponents;
using GameEngine.Core.Components;
using UISample.Components.UIComponents;

namespace UISample.Scenes;

internal class UIScene : Scene
{
	public UIScene()
	{
		UIObject cameraParent = new UIObject();

		UIButton button = new TestButton();
		button.Transform.Scale /= 4;
		uiObjects.Add(button);
		uiObjects.Add(new UITextBox());

		uiObjects.Add(cameraParent);
		uiCameras.Add((new UICamera(cameraParent), new ViewPort(0.5f, 0.5f, 1, 1)));
	}
}