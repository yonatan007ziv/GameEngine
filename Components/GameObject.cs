using OpenGLRenderer.OpenGL.Meshes;

namespace OpenGLRenderer.Components;

internal class GameObject
{
	public int Id { get; }

	public Transform Transform { get; }
	public Mesh? Mesh;
	public GizmosBoxMesh? Box;

	public GameObject(int id)
	{
		Id = id;
		Transform = new Transform();
	}

	public void Render(Camera camera)
	{
		Mesh?.Render(Transform, camera);
		Box?.Render(Transform, camera);
	}
}