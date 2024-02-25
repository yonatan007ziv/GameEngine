using GameEngine.Core.Components;

namespace GraphicsEngine.Components.Shared;

internal class RenderedUIObject
{
	public int Id { get; }

	public List<MeshRenderer> Meshes { get; } = new List<MeshRenderer>();
	public Transform Transform { get; }

	public RenderedUIObject(int id, Transform transform, params MeshRenderer[] meshRenderers)
	{
		Id = id;
		Transform = transform;
		Meshes.AddRange(meshRenderers);

		Update();
	}

	public void Render(UICamera camera)
	{
		foreach (MeshRenderer meshRenderer in Meshes)
			meshRenderer.Render(camera);
	}

	public void Update()
	{
		foreach (MeshRenderer meshRenderer in Meshes)
			meshRenderer.Update(Transform);
	}
}