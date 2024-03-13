using GameEngine.Core.Components;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace GameEngine.Components.Objects;

public abstract class UIObject
{
	public int Id { get; }
	public readonly List<UIComponent> components = new List<UIComponent>();

	public bool SyncGraphics { get; set; } = true;
	public bool SyncPhysics { get; set; }
	public bool SyncSound { get; set; }

	public bool TextDirty { get; private set; }
	public readonly Text TextData;

	public bool TransformDirty { get; private set; }
	public Transform Transform { get; }

	public bool MeshesDirty { get; private set; }
	public ObservableCollection<MeshData> Meshes { get; }

	public UIObject()
	{
		Id = IdGenerator.GenerateNext();

		TextData = new Text("", "Arial", 12);
		TextData.PropertyChanged += TextChanged;

		Transform = new Transform();
		Transform.PropertyChanged += TransformChanged;

		Meshes = new ObservableCollection<MeshData>();
		Meshes.CollectionChanged += MeshesChanged;
	}

	private void TextChanged(object? sender, PropertyChangedEventArgs e)
	{
		TextDirty = true;
		SyncGraphics = true;
	}

	private void TransformChanged(object? sender, PropertyChangedEventArgs e)
	{
		TransformDirty = true;
		SyncPhysics = true;
		SyncGraphics = true;
	}

	private void MeshesChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		MeshesDirty = true;
		SyncGraphics = true;
	}
}