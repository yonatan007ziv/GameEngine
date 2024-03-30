using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace GameEngine.Core.Components.Objects;

public abstract class UIObject
{
	public int Id { get; }
	public UIObject? Parent { get; set; }
	public string Tag { get; protected set; } = "";
	public bool Visible { get; set; } = true;

	public ObservableCollection<UIObject> Children { get; } = new ObservableCollection<UIObject>();

	public readonly TextData TextData;

	public string Text { get => TextData.Text; set => TextData.Text = value; }
	public string FontName { get => TextData.FontName; set => TextData.FontName = value; }
	public float FontSize { get => TextData.FontSize; set => TextData.FontSize = value; }

	public Transform Transform { get; set; }

	public ObservableCollection<MeshData> Meshes { get; }

	public UIObject()
	{
		Id = IdGenerator.GenerateNext();

		TextData = new TextData();
		Transform = new Transform();
		Meshes = new ObservableCollection<MeshData>();
		Children.CollectionChanged += ChildrenChanged;
	}

	public UIObject(UIObject parent)
	{
		Id = IdGenerator.GenerateNext();

		Parent = parent;
		TextData = new TextData();
		Transform = new Transform();
		Meshes = new ObservableCollection<MeshData>();
	}

	private void ChildrenChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		// New children was added
		if (e.Action == NotifyCollectionChangedAction.Add)
			foreach (UIObject addedUIObject in e.NewItems!)
				addedUIObject.Parent = this;
		// Old children was removed
		else if (e.Action == NotifyCollectionChangedAction.Remove)
			foreach (UIObject addedUIObject in e.OldItems!)
				addedUIObject.Parent = null;
	}
}