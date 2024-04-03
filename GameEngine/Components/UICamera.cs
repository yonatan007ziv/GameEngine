using GameEngine.Components.ScriptableObjects;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;

namespace GameEngine.Components;

public class UICamera : ScriptableUIObject
{
	public CameraRenderingMask RenderingMaskTags { get; } = new CameraRenderingMask();

	public UICamera() { }
	public UICamera(UIObject parent)
		: base(parent) { }

	public override void Update(float deltaTime) { }
}