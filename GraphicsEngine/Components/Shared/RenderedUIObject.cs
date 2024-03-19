using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;
using GameEngine.Core.SharedServices.Interfaces;

namespace GraphicsEngine.Components.Shared;

internal class RenderedUIObject
{
	private readonly UIObject uiObject;
	private readonly IFactory<string, string, MeshRenderer> meshFactory;

	private bool shouldUpdateMeshes;

	public int Id => uiObject.Id;
	public TextData TextData => uiObject.TextData;
	public Transform Transform => uiObject.Transform;

	public List<MeshRenderer> Meshes { get; } = new List<MeshRenderer>();

	public RenderedUIObject(UIObject uiObject, IFactory<string, string, MeshRenderer> meshFactory)
	{
		this.uiObject = uiObject;
		this.meshFactory = meshFactory;

		// Occurs from the update thread, cannot update mesh renderers from here, hence the shouldUpdateMeshes flag
		uiObject.Meshes.CollectionChanged += (s, e) => { shouldUpdateMeshes = true; };
		uiObject.Transform.PropertyChanged += (s, e) => Update();

		UpdateMeshes();
		Update();
	}

	private void UpdateMeshes()
	{
		Meshes.Clear();
		for (int i = 0; i < uiObject.Meshes.Count; i++)
			if (meshFactory.Create(uiObject.Meshes[i].Model, uiObject.Meshes[i].Material, out MeshRenderer meshRenderer))
				Meshes.Add(meshRenderer);
			else
				Console.WriteLine("Error creating MeshRenderer: {0}, {1}", uiObject.Meshes[i].Model, uiObject.Meshes[i].Material);
		Update();
	}

	public void Render(UICamera camera)
	{
		if (shouldUpdateMeshes)
		{
			UpdateMeshes();
			shouldUpdateMeshes = false;
		}

		foreach (MeshRenderer meshRenderer in Meshes)
			meshRenderer.Render(camera);
	}

	public void Update()
	{
		foreach (MeshRenderer meshRenderer in Meshes)
			meshRenderer.Update(Transform);
	}
}