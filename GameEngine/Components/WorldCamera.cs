using GameEngine.Components.ScriptableObjects;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;

namespace GameEngine.Components;

public class WorldCamera : ScriptableWorldObject
{
	// Objects that have the tags in this list won't be rendered from this camera
	public CameraRenderingMask RenderingMaskTags { get; } = new CameraRenderingMask();

	public WorldCamera() { }
	public WorldCamera(WorldObject parent)
		: base(parent) { }

	public override void Update(float deltaTime) { }
}