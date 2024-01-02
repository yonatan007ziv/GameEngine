using GraphicsRenderer.Components.Shared;

namespace GraphicsRenderer.Components.Interfaces;

public interface IMesh
{
	public void Render(ICamera camera, Material material);
	public void Update(Transform transform);
}