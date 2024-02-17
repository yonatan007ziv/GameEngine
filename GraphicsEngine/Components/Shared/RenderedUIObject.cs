using GameEngine.Core.Components;
using GraphicsEngine.Components.Interfaces;

namespace GraphicsEngine.Components.Shared;

internal class RenderedUIObject
{
	public int Id { get; }

	public List<IMeshRenderer> Meshes { get; } = new List<IMeshRenderer>();
	public Transform Transform { get; }

	public RenderedUIObject(int id, Transform transform, params IMeshRenderer[] meshRenderers)
	{
		Id = id;
		Transform = transform;
		Meshes.AddRange(meshRenderers);

		Update();
	}

	public void Render(UICamera camera)
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