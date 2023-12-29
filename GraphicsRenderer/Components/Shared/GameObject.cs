using GraphicsRenderer.Components.Interfaces;

namespace GraphicsRenderer.Components.Shared;

internal class GameObject
{
	public int Id { get; }

	public IShaderProgram shader { get; set; }
	public Transform Transform { get; }
	public IMesh? Mesh;
	public IMesh? Gizmos;

	public GameObject(int id)
	{
		Id = id;
		Transform = new Transform();
	}

	public void Render(ICamera camera)
	{
		Mesh?.Render(Transform, camera, shader);
		Gizmos?.Render(Transform, camera, shader);
	}
}