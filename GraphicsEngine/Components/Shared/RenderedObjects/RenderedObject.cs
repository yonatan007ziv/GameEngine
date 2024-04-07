using GameEngine.Core.Components.Objects;
using GameEngine.Core.SharedServices.Interfaces;
using System.Collections.ObjectModel;

namespace GraphicsEngine.Components.Shared.RenderedObjects;

internal abstract class RenderedObject
{
	protected readonly IFactory<string, string, MeshRenderer> meshFactory;

	protected bool shouldUpdateMeshes;
	protected bool shouldUpdateChildren;

	protected GameObject gameObject { get; }
	protected GameObject? parent { get; set; }

	public List<MeshRenderer> Meshes { get; } = new List<MeshRenderer>();
	public ObservableCollection<RenderedObject> Children { get; } = new ObservableCollection<RenderedObject>();

	protected RenderedObject(GameObject gameObject, IFactory<string, string, MeshRenderer> meshFactory, GameObject? parent = null)
	{
		this.gameObject = gameObject;
		this.parent = parent;
		this.meshFactory = meshFactory;

		// Occurs from the update thread, cannot update mesh renderers from here, hence the shouldUpdateMeshes flag
		this.gameObject.Meshes.CollectionChanged += (s, e) => { shouldUpdateMeshes = true; };
		this.gameObject.Children.CollectionChanged += (s, e) => { shouldUpdateChildren = true; };

		this.gameObject.Transform.PropertyChanged += (s, e) => UpdateChildTree();

		UpdateMeshes();
		UpdateChildren();
		UpdateChildTree();
	}

	protected abstract void UpdateMeshes();
	protected abstract void UpdateChildren();

	public void Render(RenderingCamera camera)
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

	public void UpdateChildTree()
	{
		foreach (MeshRenderer meshRenderer in Meshes)
			meshRenderer.Update(gameObject);

		foreach (RenderedObject child in Children)
			child.UpdateChildTree();
	}
}