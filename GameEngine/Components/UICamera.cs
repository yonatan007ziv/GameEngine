using GameEngine.Components.ScriptableObjects;
using GameEngine.Components.UIComponents;
using GameEngine.Core.Components.Objects;

namespace GameEngine.Components;

public class UICamera : ScriptableUIComponent
{
    public bool Standalone { get; }

    public UICamera()
        : base(new UIEmptyElement())
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