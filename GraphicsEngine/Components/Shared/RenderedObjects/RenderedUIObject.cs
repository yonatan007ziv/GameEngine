using GameEngine.Core.Components.Objects;
using GameEngine.Core.SharedServices.Interfaces;

namespace GraphicsEngine.Components.Shared.RenderedObjects;

internal class RenderedUIObject : RenderedObject
{
	public UIObject UIObject => (gameObject as UIObject) ?? throw new Exception();

	public RenderedUIObject(UIObject uiObject, IFactory<string, string, MeshRenderer> meshFactory, UIObject? parent = null)
		: base(uiObject, meshFactory, parent)
	{

	}

	protected override void UpdateMeshes()
	{
		Meshes.Clear();
		for (int i = 0; i < gameObject.Meshes.Count; i++)
			if (meshFactory.Create(UIObject.Meshes[i].Model, UIObject.Meshes[i].Material, out MeshRenderer meshRenderer))
				Meshes.Add(meshRenderer);
			else
				Console.WriteLine("Error creating MeshRenderer: {0}, {1}", UIObject.Meshes[i].Model, UIObject.Meshes[i].Material);
		UpdateChildTree();
	}

	protected override void UpdateChildren()
	{
		Children.Clear();
		for (int i = 0; i < UIObject.Children.Count; i++)
			if (UIObject.Children[i] is UIObject childUIObject)
				Children.Add(new RenderedUIObject(childUIObject, meshFactory, UIObject));
	}
}