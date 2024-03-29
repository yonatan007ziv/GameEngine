using System.Collections.ObjectModel;

namespace GameEngine.Core.Components.Objects;

public class UIComponent
{
	public int Id { get; }
	public UIObject Parent { get; }

	protected Transform Transform;
	protected ObservableCollection<MeshData> Meshes;

	public UIComponent(UIObject parent)
	{
		Id = IdGenerator.GenerateNext();
		Parent = parent;

		parent.components.Add(this);

		Transform = parent.Transform;
		Meshes = parent.Meshes;
	}
}