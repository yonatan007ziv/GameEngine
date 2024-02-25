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

		UIButton button1 = new TestButton();
		button1.Transform.Scale /= 4;
		uiObjects.Add(button1);

		UIButton button2 = new TestButton();
		button2.Transform.Scale /= 10;
		button2.Transform.Position = new System.Numerics.Vector3(0.75f, 0.75f, 0);
		uiObjects.Add(button2);

		uiObjects.Add(cameraParent);
		uiCameras.Add((new UICamera(cameraParent), new ViewPort(0.5f, 0.5f, 1, 1)));
	}
}