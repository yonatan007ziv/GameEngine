using GameEngine.Core.Components;

namespace GraphicsEngine.Components.Shared;

internal class RenderedWorldObject
{
	public int Id { get; }

	public List<MeshRenderer> Meshes { get; } = new List<MeshRenderer>();
	public Transform Transform { get; }

	public RenderedWorldObject(int id, Transform transform, params MeshRenderer[] meshRenderers)
	{
		Id = id;
		Transform = transform;
		Meshes.AddRange(meshRenderers);

		Update();
	}

	public void Render(WorldCamera camera)
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