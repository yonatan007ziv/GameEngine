using System.Collections.ObjectModel;

namespace GameEngine.Core.Components.Objects;

public abstract class UIObject
{
	public int Id { get; }
	public readonly List<UIComponent> components = new List<UIComponent>();
	public readonly TextData TextData;
	public string Text { get => TextData.Text; set => TextData.Text = value; }
	public string FontName { get => TextData.FontName; set => TextData.FontName = value; }
	public float FontSize { get => TextData.FontSize; set => TextData.FontSize = value; }

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