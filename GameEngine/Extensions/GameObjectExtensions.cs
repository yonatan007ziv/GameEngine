using GameEngine.Components;
using GameEngine.Core.Components;
using GameEngine.Core.Extensions;

namespace GameEngine.Extensions;

internal static class GameObjectExtensions
{
	public static GameObjectData TranslateGameObject(this GameObject gameObject)
		=> new GameObjectData(gameObject.Id, gameObject.IsUI, gameObject.Transform.TranslateTransform(), gameObject.TransformDirty,
			gameObject.Meshes.ToList(), gameObject.MeshesDirty,
			gameObject.Forces.ToList(), gameObject.ForcesDirty,
			gameObject.ImpulseVelocities.ToList(), gameObject.ImpulseVelocitiesDirty);
}