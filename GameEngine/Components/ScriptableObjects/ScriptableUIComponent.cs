using GameEngine.Core.Components.Objects;

namespace GameEngine.Components.ScriptableObjects;

public abstract class ScriptableUIComponent : UIComponent
{
    public ScriptableUIComponent(UIObject parent)
        : base(parent)
    {
    }

    public abstract void Update(float deltaTime);
}