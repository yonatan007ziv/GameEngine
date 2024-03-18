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
	public bool SyncSound { get; set; }

	public bool SyncPhysics { get; set; }
	public readonly TextData TextData;

	public bool TransformDirty { get; private set; }
	public Transform Transform { get; }

	public bool MeshesDirty { get; private set; }
	public ObservableCollection<MeshData> Meshes { get; }

	public UIObject()
	{
		Id = IdGenerator.GenerateNext();

		// Default font
		TextData = new TextData(default!);

		Transform = new Transform();
		Transform.PropertyChanged += TransformChanged;

		Meshes = new ObservableCollection<MeshData>();
		Meshes.CollectionChanged += MeshesChanged;
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