using GameEngine.Components;

namespace SampleFont;

internal class FontTestingScene : Scene
{
	public FontTestingScene()
	{
		UICameras.Add((new UICamera(), new GameEngine.Core.Components.ViewPort(0.5f, 0.5f, 1, 1)));

		// TestingMovingElement movingElement = new TestingMovingElement(0);
		// movingElement.EnabledMovement = true;
		// movingElement.Transform.Scale /= 4;
		// movingElement.Transform.Position += new System.Numerics.Vector3(0.5f, 0, 0);
		// UIObjects.Add(movingElement);

		TestingRecursiveElement recursiveElement = new TestingRecursiveElement(5);
		recursiveElement.Transform.Scale /= 2;
		UIObjects.Add(recursiveElement);
	}
}