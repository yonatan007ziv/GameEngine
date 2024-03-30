using GameEngine.Components;

namespace SampleFont;

internal class FontTestingScene : Scene
{
	public FontTestingScene()
	{
		UICameras.Add((new UICamera(), new GameEngine.Core.Components.ViewPort(0.5f, 0.5f, 1, 1)));

		TestingMovingElement lobbyButton = new TestingMovingElement("hello", "goodbyte", "Some text", 1, 2);

		lobbyButton.Transform.Scale /= 4;
		lobbyButton.Transform.Position += new System.Numerics.Vector3(0.5f, 0, 0);

		UIObjects.Add(lobbyButton);
	}
}