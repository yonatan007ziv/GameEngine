namespace GameEngine.Components.Objects.Scriptable;

public abstract class ScriptableUIComponent : UIComponent
{
	public ScriptableUIComponent(UIObject parent)
		: base(parent)
	{
	}

	public abstract void Update(float deltaTime);
}