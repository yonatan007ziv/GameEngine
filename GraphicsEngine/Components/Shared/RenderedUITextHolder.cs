using GameEngine.Core.Components;

namespace GraphicsEngine.Components.Shared;

internal class RenderedUITextHolder : RenderedUIObject
{
	public int FontSize { get; set; }
	public string Text { get; set; }

	public RenderedUITextHolder(int id, Transform transform, params MeshRenderer[] meshRenderers)
		: base(id, transform, meshRenderers)
	{

	}
}