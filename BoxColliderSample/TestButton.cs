using GameEngine.Components.UIComponents;

namespace BoxColliderSample;

internal class TestButton : UIButton
{
	public TestButton()
	{
		OnScreenSizeChanged += (vec) =>
		{
			Console.WriteLine($"Changed screen size: {vec}");
		};
	}
}