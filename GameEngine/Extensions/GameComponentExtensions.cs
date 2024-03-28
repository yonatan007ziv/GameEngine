using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;

namespace GameEngine.Extensions;

internal static class GameComponentExtensions
{
    public static GameComponentData TranslateWorldComponent(this WorldComponent worldComponent)
        => new GameComponentData(worldComponent.Id, worldComponent.Parent.Id);
    public static GameComponentData TranslateUIComponent(this UIComponent uiComponent)
        => new GameComponentData(uiComponent.Id, uiComponent.Parent.Id);
}