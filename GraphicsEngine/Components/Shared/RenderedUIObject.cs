using GameEngine.Core.Components.Objects;
using GameEngine.Core.SharedServices.Interfaces;

namespace GraphicsEngine.Components.Shared;

internal class RenderedUIObject
{
	private readonly IFactory<string, string, MeshRenderer> meshFactory;

	private bool shouldUpdateMeshes;
	private bool shouldUpdateChildren;

	public UIObject UIObject { get; }
	public List<RenderedUIObject> Children { get; } = new List<RenderedUIObject>();
	public List<MeshRenderer> Meshes { get; } = new List<MeshRenderer>();
	public UIObject? Parent { get; }

	public RenderedUIObject(UIObject uiObject, IFactory<string, string, MeshRenderer> meshFactory, UIObject? parent = null)
	{
		this.meshFactory = meshFactory;
		UIObject = uiObject;
		Parent = parent;

		// Occurs from the update thread, cannot update mesh renderers from here, hence the shouldUpdateMeshes flag
		uiObject.Meshes.CollectionChanged += (s, e) => { shouldUpdateMeshes = true; };
		uiObject.Children.CollectionChanged += (s, e) => { shouldUpdateChildren = true; };
		uiObject.Transform.PropertyChanged += (s, e) => Update();
		if (parent is not null)
			parent.Transform.PropertyChanged += (s, e) => Update();

		UpdateMeshes();
		UpdateChildren();
		Update();
	}

	private void UpdateMeshes()
	{
		Meshes.Clear();
		for (int i = 0; i < UIObject.Meshes.Count; i++)
			if (meshFactory.Create(UIObject.Meshes[i].Model, UIObject.Meshes[i].Material, out MeshRenderer meshRenderer))
				Meshes.Add(meshRenderer);
			else
				Console.WriteLine("Error creating MeshRenderer: {0}, {1}", UIObject.Meshes[i].Model, UIObject.Meshes[i].Material);
		Update();
	}
	private void UpdateChildren()
	{
		Children.Clear();
		foreach (UIObject child in UIObject.Children)
			Children.Add(new RenderedUIObject(child, meshFactory, UIObject));
		Update();
	}
	public void Render(RenderingUICamera camera)
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

		if (UIObject.Visible && (Parent?.Visible ?? true))
			foreach (MeshRenderer meshRenderer in Meshes)
				meshRenderer.Render(camera);
	}

	public void Update()
	{
		foreach (MeshRenderer meshRenderer in Meshes)
		{
			if (Parent is not null)
				meshRenderer.Update(UIObject.Transform, Parent.Transform);
			else
				meshRenderer.Update(UIObject.Transform);
		}
	}
}