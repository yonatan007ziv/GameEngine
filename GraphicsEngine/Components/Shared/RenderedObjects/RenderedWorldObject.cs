using GameEngine.Core.Components.Objects;
using GameEngine.Core.SharedServices.Interfaces;

namespace GraphicsEngine.Components.Shared.RenderedObjects;

internal class RenderedWorldObject : RenderedObject
{
	public WorldObject WorldObject => (gameObject as WorldObject) ?? throw new Exception();

	public RenderedWorldObject(WorldObject worldObject, IFactory<string, string, MeshRenderer> meshFactory, WorldObject? parent = null)
		: base(worldObject, meshFactory)
	{

	}

	protected override void UpdateMeshes()
	{
		Meshes.Clear();
		for (int i = 0; i < gameObject.Meshes.Count; i++)
			if (meshFactory.Create(WorldObject.Meshes[i].Model, WorldObject.Meshes[i].Material, out MeshRenderer meshRenderer))
				Meshes.Add(meshRenderer);
			else
				Console.WriteLine("Error creating MeshRenderer: {0}, {1}", WorldObject.Meshes[i].Model, WorldObject.Meshes[i].Material);
		Update();
	}

	protected override void UpdateChildren()
	{
		Children.Clear();
		for (int i = 0; i < WorldObject.Children.Count; i++)
			if (WorldObject.Children[i] is WorldObject childWorldObject)
				Children.Add(new RenderedWorldObject(childWorldObject, meshFactory, WorldObject));
	}
}