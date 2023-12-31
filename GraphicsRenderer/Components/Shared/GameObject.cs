using GraphicsRenderer.Components.Interfaces;

namespace GraphicsRenderer.Components.Shared;

internal class GameObject
{
	public int Id { get; }

	public Material Material { get; set; }
	public Transform Transform { get; }
	public IMesh? Mesh;
	public IMesh? Gizmos;

	public GameObject(Material material, int id)
	{
		Material = material;
		Id = id;
		Transform = new Transform();
	}

	public void Render(ICamera camera)
	{
		Mesh?.Render(Transform, camera, Material);
		Gizmos?.Render(Transform, camera, Material);
	}
}