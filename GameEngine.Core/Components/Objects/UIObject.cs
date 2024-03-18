using System.Collections.ObjectModel;

namespace GameEngine.Core.Components.Objects;

public abstract class UIObject
{
	public int Id { get; }
	public readonly List<UIComponent> components = new List<UIComponent>();
	public readonly TextData TextData;

	public Transform Transform { get; }
	public ObservableCollection<MeshData> Meshes { get; }

	public UIObject()
	{
		Id = IdGenerator.GenerateNext();

		// Default font
		TextData = new TextData(default!);

		Transform = new Transform();
		Meshes = new ObservableCollection<MeshData>();
	}
}