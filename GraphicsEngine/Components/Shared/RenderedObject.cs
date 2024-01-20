using GameEngine.Core.Components;
using GraphicsEngine.Components.Interfaces;

namespace GraphicsEngine.Components.Shared;

internal class RenderedObject
{
	public int Id { get; }

	public List<IMeshRenderer> Meshes = new List<IMeshRenderer>();
	public Transform Transform { get; }

	public RenderedObject(Transform transform, int id, params IMeshRenderer[] meshRenderers)
	{
		Id = id;
		Transform = transform;
		Meshes.AddRange(meshRenderers);
	}

	public void Render(RenderedCamera camera)
	{
		foreach (IMeshRenderer meshRenderer in Meshes)
			meshRenderer.Render(camera);
	}

	public virtual void Update()
	{
		foreach (IMeshRenderer meshRenderer in Meshes)
			meshRenderer.Update(Transform);
	}
}