using GameEngine.Components;
using GameEngine.Core.Components;

namespace GameEngine.Extensions;

internal static class GameComponentExtensions
{
	public static GameComponentData TranslateGameComponent(this GameComponent gameComponent)
		=> new GameComponentData(gameComponent.Id, gameComponent.ParentId, gameComponent.IsUI);
}