using GameEngine.Core.Components;
using GraphicsEngine.Components.Interfaces;

namespace GraphicsEngine.Components.Shared;

internal class RenderedObject : IObject
{
	public int Id { get; }

	public List<IMeshRenderer> Meshes { get; } = new List<IMeshRenderer>();
	public Transform Transform { get; }

	public RenderedObject(int id, Transform transform, params IMeshRenderer[] meshRenderers)
	{
		Id = id;
		Transform = transform;
		Meshes.AddRange(meshRenderers);
	}

	public void Render(Camera camera)
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