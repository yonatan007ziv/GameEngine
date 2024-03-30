using GameEngine.Core.Components.Objects;
using GameEngine.Core.SharedServices.Interfaces;

namespace GraphicsEngine.Components.Shared;

internal class RenderedWorldObject
{
	private readonly IFactory<string, string, MeshRenderer> meshFactory;

	private bool shouldUpdateMeshes;
	private bool shouldUpdateChildren;

	public WorldObject WorldObject { get; }
	public List<RenderedWorldObject> Children { get; } = new List<RenderedWorldObject>();
	public List<MeshRenderer> Meshes { get; } = new List<MeshRenderer>();
	public WorldObject? Parent { get; }

	public RenderedWorldObject(WorldObject worldObject, IFactory<string, string, MeshRenderer> meshFactory, WorldObject? parent = null)
	{
		this.meshFactory = meshFactory;
		WorldObject = worldObject;
		Parent = parent;

		// Occurs on the update thread, cannot update mesh renderers from here, hence the shouldUpdateMeshes flag
		worldObject.Meshes.CollectionChanged += (s, e) => { shouldUpdateMeshes = true; };
		worldObject.Children.CollectionChanged += (s, e) => { shouldUpdateChildren = true; };
		worldObject.Transform.PropertyChanged += (s, e) => Update();
		if (parent is not null)
			parent.Transform.PropertyChanged += (s, e) => Update();

		UpdateMeshes();
		UpdateChildren();
		Update();
	}

	private void UpdateMeshes()
	{
		Meshes.Clear();
		for (int i = 0; i < WorldObject.Meshes.Count; i++)
			if (meshFactory.Create(WorldObject.Meshes[i].Model, WorldObject.Meshes[i].Material, out MeshRenderer meshRenderer))
				Meshes.Add(meshRenderer);
			else
				Console.WriteLine("Error creating MeshRenderer: {0}, {1}", WorldObject.Meshes[i].Model, WorldObject.Meshes[i].Material);
		Update();
	}
	private void UpdateChildren()
	{
		Children.Clear();
		foreach (WorldObject child in WorldObject.Children)
			Children.Add(new RenderedWorldObject(child, meshFactory, WorldObject));
	}
	public void Render(RenderingWorldCamera camera)
	{
		if (shouldUpdateMeshes)
		{
			UpdateMeshes();
			shouldUpdateMeshes = false;
		}
		if (shouldUpdateChildren)
		{
			UpdateChildren();
			shouldUpdateChildren = false;
		}

		foreach (MeshRenderer meshRenderer in Meshes)
			meshRenderer.Render(camera);
	}

	public void Update()
	{
		foreach (MeshRenderer meshRenderer in Meshes)
		{
			if (Parent is not null)
				meshRenderer.Update(WorldObject.Transform, Parent.Transform);
			else
				meshRenderer.Update(WorldObject.Transform);
		}
	}
}