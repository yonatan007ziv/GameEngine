using GameEngine.Components;

namespace SampleFont;

internal class FontTestingScene : Scene
{
	public FontTestingScene()
	{
		UICameras.Add((new UICamera(), new GameEngine.Core.Components.ViewPort(0.5f, 0.5f, 1, 1)));

		TestingMovingElement recursiveElement = new TestingMovingElement(0);

		recursiveElement.EnabledMovement = true;
		recursiveElement.Transform.Scale /= 4;
		recursiveElement.Transform.Position += new System.Numerics.Vector3(0.5f, 0, 0);

		UIObjects.Add(recursiveElement);
	}
}