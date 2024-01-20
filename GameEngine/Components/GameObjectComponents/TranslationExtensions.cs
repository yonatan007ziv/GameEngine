using GameEngine.Components.GameObjectComponents;

namespace GameEngine.Core.Components.CommunicationComponentsData;

internal static class TranslationExtensions
{
	public static GameObjectData TranslateGameObject(this GameObject gameObject)
		=> new GameObjectData(gameObject.Id, gameObject.Transform, gameObject.Meshes.ToList());
}