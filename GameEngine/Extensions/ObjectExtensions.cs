using GameEngine.Components.Objects;
using GameEngine.Core.Components;
using GameEngine.Core.Extensions;

namespace GameEngine.Extensions;

internal static class ObjectExtensions
{
	public static WorldObjectData TranslateWorldObject(this WorldObject worldObject)
		=> new WorldObjectData(worldObject.Id, worldObject.Transform.TranslateTransform(), worldObject.TransformDirty,
			worldObject.Meshes.ToList(), worldObject.MeshesDirty,
			worldObject.Forces.ToList(), worldObject.ForcesDirty,
			worldObject.ImpulseVelocities.ToList(), worldObject.ImpulseVelocitiesDirty);

	public static UIObjectData TranslateUIObject(this UIObject uiObject)
		=> new UIObjectData(uiObject.Id, uiObject.Transform.TranslateTransform(), uiObject.TransformDirty,
			uiObject.Meshes.ToList(), uiObject.MeshesDirty);
}