using GameEngine.Core.Components;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Components.Shared.Data;

namespace GraphicsEngine.Components.Interfaces;

public interface IMeshRenderer
{
	ModelData Model { get; set; }
	Material Material { get; set; }

	public void Render(RendererCamera camera);
	public void Update(Transform transform);
}