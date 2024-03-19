using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;
using GameEngine.Core.SharedServices.Interfaces;

namespace GraphicsEngine.Components.Shared;

internal class RenderedWorldObject
{
	private readonly WorldObject worldObject;
	private readonly IFactory<string, string, MeshRenderer> meshFactory;

	private bool shouldUpdateMeshes;

	public int Id => worldObject.Id;
	public Transform Transform => worldObject.Transform;
	public List<MeshRenderer> Meshes { get; } = new List<MeshRenderer>();

	public RenderedWorldObject(WorldObject worldObject, IFactory<string, string, MeshRenderer> meshFactory)
	{
		this.worldObject = worldObject;
		this.meshFactory = meshFactory;

		// Occurs on the update thread, cannot update mesh renderers from here, hence the shouldUpdateMeshes flag
		worldObject.Meshes.CollectionChanged += (s, e) => { shouldUpdateMeshes = true; };
		worldObject.Transform.PropertyChanged += (s, e) => Update();

		UpdateMeshes();
		Update();
	}

	private void UpdateMeshes()
	{
		Meshes.Clear();
		for (int i = 0; i < worldObject.Meshes.Count; i++)
			if (meshFactory.Create(worldObject.Meshes[i].Model, worldObject.Meshes[i].Material, out MeshRenderer meshRenderer))
				Meshes.Add(meshRenderer);
			else
				Console.WriteLine("Error creating MeshRenderer: {0}, {1}", worldObject.Meshes[i].Model, worldObject.Meshes[i].Material);
		Update();
	}
	public void Render(WorldCamera camera)
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