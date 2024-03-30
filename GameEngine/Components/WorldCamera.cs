using GameEngine.Components.ScriptableObjects;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;

namespace GameEngine.Components;

public class WorldCamera : ScriptableWorldObject
{
	public CameraRenderingMask<string> RenderingMaskTags { get; } = new CameraRenderingMask<string>();

	public WorldCamera() { }
	public WorldCamera(WorldObject parent)
		: base(parent) { }

	public override void Update(float deltaTime) { }
}