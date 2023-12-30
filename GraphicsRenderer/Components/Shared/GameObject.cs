using GraphicsRenderer.Components.Interfaces;

namespace GraphicsRenderer.Components.Shared;

internal class GameObject
{
	public int Id { get; }

	// change to material later on to encapsulate textures?
	public IShaderProgram Shader { get; set; }
	public Transform Transform { get; }
	public IMesh? Mesh;
	public IMesh? Gizmos;

	public GameObject(IShaderProgram defaultShader, int id)
	{
		Shader = defaultShader;
		Id = id;
		Transform = new Transform();
	}

	public void Render(ICamera camera)
	{
		Mesh?.Render(Transform, camera, Shader);
		Gizmos?.Render(Transform, camera, Shader);
	}
}