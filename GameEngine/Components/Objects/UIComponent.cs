using GameEngine.Core.Components;
using System.Collections.ObjectModel;

namespace GameEngine.Components.Objects;

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

	public void Dispose()
	{
		throw new NotImplementedException();
	}
}