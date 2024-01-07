using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Components.Shared.Data;

namespace GraphicsRenderer.Components.Interfaces;

public interface IMeshRenderer
{
	ModelData Model { get; set; }
	Material Material { get; set; }

	public void Render(ICamera camera);
	public void Update(Transform transform);
}