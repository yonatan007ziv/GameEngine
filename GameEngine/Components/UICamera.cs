using GameEngine.Components.ScriptableObjects;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;

namespace GameEngine.Components;

public class UICamera : ScriptableUIObject
{
	public CameraRenderingMask<string> RenderingMaskTags { get; } = new CameraRenderingMask<string>();

	public UICamera() { }
	public UICamera(UIObject parent)
		: base(parent) { }

	public override void Update(float deltaTime) { }
}