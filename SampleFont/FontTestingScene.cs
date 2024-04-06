using GameEngine.Components;

namespace SampleFont;

internal class FontTestingScene : Scene
{
	public FontTestingScene()
	{
		UICameras.Add((new UICamera(), new GameEngine.Core.Components.ViewPort(0.5f, 0.5f, 1, 1)));

		TestingRecursiveElement recursiveElement = new TestingRecursiveElement(0);
		recursiveElement.Transform.Scale /= 2;
		UIObjects.Add(recursiveElement);
	}
}