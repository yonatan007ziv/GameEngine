using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;
using GameEngine.Core.SharedServices.Interfaces;

namespace GraphicsEngine.Components.Shared;

internal class RenderedUIObject
{
	private readonly UIObject uiObject;
	private readonly IFactory<string, string, MeshRenderer> meshFactory;

	public int Id => uiObject.Id;
	public TextData TextData => uiObject.TextData;
	public Transform Transform => uiObject.Transform;

	public List<MeshRenderer> Meshes { get; } = new List<MeshRenderer>();

	public RenderedUIObject(UIObject uiObject, IFactory<string, string, MeshRenderer> meshFactory)
	{
		this.uiObject = uiObject;
		this.meshFactory = meshFactory;

		UpdateMeshRenderers();
		uiObject.Meshes.CollectionChanged += (s, e) => UpdateMeshRenderers();

		Update();
	}

	private void UpdateMeshRenderers()
	{
		Meshes.Clear();
		for (int i = 0; i < uiObject.Meshes.Count; i++)
			if (meshFactory.Create(uiObject.Meshes[i].Model, uiObject.Meshes[i].Material, out MeshRenderer meshRenderer))
				Meshes.Add(meshRenderer);
			else
				Console.WriteLine("Error creating MeshRenderer: {0}, {1}", uiObject.Meshes[i].Model, uiObject.Meshes[i].Material);
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