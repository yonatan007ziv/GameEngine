using GameEngine.Core.Components;
using GameEngine.Core.Components.Fonts;

namespace GraphicsEngine.Components.Shared;

internal class RenderedUIObject
{
	public int Id { get; }

	public TextData TextData;

	public List<MeshRenderer> Meshes { get; } = new List<MeshRenderer>();
	public Transform Transform { get; }

	public RenderedUIObject(int id, Transform transform, TextData text, params MeshRenderer[] meshRenderers)
	{
		Id = id;
		Transform = transform;
		TextData = text;
		Meshes.AddRange(meshRenderers);

		Update();
	}

	public void Render(UICamera camera)
	{
		RenderMeshes(camera);
		RenderText(camera);
	}

	public void Update()
	{
		foreach (MeshRenderer meshRenderer in Meshes)
			meshRenderer.Update(Transform);
	}

	private void RenderMeshes(UICamera camera)
	{
		foreach (MeshRenderer meshRenderer in Meshes)
			meshRenderer.Render(camera);
	}

	private void RenderText(UICamera camera)
	{
		
	}
}