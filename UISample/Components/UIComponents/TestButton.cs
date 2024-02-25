using GameEngine.Components.UIComponents;
using GameEngine.Core.Components;

namespace UISample.Components.UIComponents;

internal class TestButton : UIButton
{
	protected override void OnFullClicked()
	{
		Meshes[0] = new MeshData("UIPlane.obj", "Green.mat");
	}

	protected override void OnDragClicked()
	{
		Meshes[0] = new MeshData("UIPlane.obj", "Red.mat");
	}

	protected override void OnReleased()
	{

	}

	protected override void OnEnter()
	{
		Meshes[0] = new MeshData("UIPlane.obj", "Wall.mat");
	}

	protected override void OnExit()
	{
		Meshes[0] = new MeshData("UIPlane.obj", "Ground.mat");
	}

	public TestButton()
		: base(new MeshData("UIPlane.obj", "Trex.mat"))
	{

	}
}