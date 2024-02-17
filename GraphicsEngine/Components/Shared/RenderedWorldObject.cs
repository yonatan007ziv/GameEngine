using GameEngine.Core.Components;
using GraphicsEngine.Components.Interfaces;

namespace GraphicsEngine.Components.Shared;

internal class RenderedWorldObject
{
	public int Id { get; }

	public List<IMeshRenderer> Meshes { get; } = new List<IMeshRenderer>();
	public Transform Transform { get; }

	public RenderedWorldObject(int id, Transform transform, params IMeshRenderer[] meshRenderers)
	{
		Id = id;
		Transform = transform;
		Meshes.AddRange(meshRenderers);

		Update();
    }

	public void Render(WorldCamera camera)
	{
		foreach (IMeshRenderer meshRenderer in Meshes)
			meshRenderer.Render(camera);
	}

	public void Update()
	{
		foreach (IMeshRenderer meshRenderer in Meshes)
			meshRenderer.Update(Transform);
	}
}