using GameEngine.Components.UIComponents;

namespace BoxColliderSample;

internal class TestButton : UIButton
{
	public TestButton()
	{
		Meshes.Add(new GameEngine.Core.Components.MeshData("UIRect.obj", "texture.mat"));
		OnScreenSizeChanged += (vec) =>
		{
			Console.WriteLine($"Changed screen size: {vec}");
		};
	}
}