using GameEngine.Components.Objects;
using GameEngine.Components.UIComponents;
using GameEngine.Core.Components;

namespace GameEngine.Extensions;

internal static class ObjectExtensions
{
	public static WorldObjectData TranslateWorldObject(this WorldObject worldObject)
		=> new WorldObjectData(worldObject.Id, worldObject.Transform.TranslateTransform(), worldObject.TransformDirty,
			worldObject.BoxCollider, worldObject.BoxColliderDirty,
			worldObject.Meshes.ToList(), worldObject.MeshesDirty,
			worldObject.Forces.ToList(), worldObject.ForcesDirty,
			worldObject.ImpulseVelocities.ToList(), worldObject.ImpulseVelocitiesDirty);

	public static UIObjectData TranslateUIObject(this UIObject uiObject)
	{
		return new UIObjectData(uiObject.Id, uiObject.TextData, uiObject.Transform.TranslateTransform(), uiObject.TransformDirty,
			uiObject.Meshes.ToList(), uiObject.MeshesDirty);
	}
}