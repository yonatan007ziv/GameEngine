using System.Collections.ObjectModel;

namespace GameEngine.Core.Components.Objects;

public abstract class UIObject
{
	public int Id { get; }
	public readonly List<UIComponent> components = new List<UIComponent>();
	public readonly TextData TextData;
	public string Text { get => TextData.Text; set => TextData.Text = value; }
	public bool Visible { get; set; } = true;

	public Transform Transform { get; }
	public ObservableCollection<MeshData> Meshes { get; }

	public UIObject()
	{
		Id = IdGenerator.GenerateNext();

		// Default font
		TextData = new TextData();

		Transform = new Transform();
		Meshes = new ObservableCollection<MeshData>();
	}
}