using GameEngine.Components.Objects;
using GameEngine.Components.Objects.Scriptable;

namespace GameEngine.Components;

public class UICamera : ScriptableUIComponent
{
	public UICamera()
		: base(new UIObject())
	{

	}

	public UICamera(UIObject parent)
		: base(parent)
	{

	}

	public override void Update(float deltaTime)
	{

	}
}