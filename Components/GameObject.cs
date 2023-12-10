namespace OpenGLRenderer.Components;

internal class GameObject
{
	public int Id { get; }

	public Transform Transform { get; }
	public Mesh? Mesh;

	public GameObject(int id)
	{
		Id = id;
		Transform = new Transform();
	}

	public void Render()
	{
		Mesh?.Render(Transform);
	}
}