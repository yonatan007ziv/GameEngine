using GameEngine.Components.Objects;
using GameEngine.Components.Objects.Scriptable;

namespace GameEngine.Components;

public class UICamera : ScriptableUIComponent
{
	public bool Standalone { get; }

	public UICamera()
		: base(new UIObject())
	{
		Standalone = true;
	}

	public UICamera(UIObject parent)
		: base(parent)
	{
		Standalone = false;
	}

	public override void Update(float deltaTime)
	{

	}
}