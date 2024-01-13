using GameEngine.Core.Components.CommunicationComponentsData;

namespace GameEngine.Components.GameObjectComponents;

internal static class TranslationExtensions
{
	public static GameObjectData TranslateGameObject(this GameObject gameObject)
		=> new GameObjectData(gameObject.Id, gameObject.Transform.TranslateTransform(), gameObject.TransformDirty, gameObject.Meshes.ToList(), gameObject.MeshesDirty);

	public static TransformData TranslateTransform(this Transform transform)
		=> new TransformData(transform.Position, transform.Rotation, transform.Scale);
}