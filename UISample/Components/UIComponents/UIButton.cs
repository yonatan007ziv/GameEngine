using GameEngine.Components.Objects.Scriptable;
using GameEngine.Core.Components;

namespace UISample.Components.UIComponents;

internal class UIButton : ScriptableUIObject
{
    public UIButton()
    {
		Meshes.Add(new MeshData("UIPlane.obj", "Hamama.mat"));
		Transform.Scale /= 4;
    }

    public override void OnClicked()
	{
		Console.WriteLine("Clicked");
	}

	public override void OnHover()
	{
		Console.WriteLine("Hovering");
	}
}