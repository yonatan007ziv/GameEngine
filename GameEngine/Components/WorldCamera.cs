using GameEngine.Components.ScriptableObjects;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;

namespace GameEngine.Components;

public class WorldCamera : ScriptableWorldObject
{
	public CameraRenderingMask RenderingMaskTags { get; } = new CameraRenderingMask();

	public WorldCamera() { }
	public WorldCamera(WorldObject parent)
		: base(parent) { }

	public override void Update(float deltaTime) { }
}