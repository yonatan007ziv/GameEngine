using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Numerics;

namespace GameEngine.Core.Components.Objects;

public abstract class WorldObject
{
	public int Id { get; }
	public WorldObject? Parent { get; set; }
	public string Tag { get; protected set; } = "";
	public bool Visible { get; set; } = true;

	public readonly ObservableCollection<WorldObject> Children = new ObservableCollection<WorldObject>();

	public Transform Transform { get; set; }

	public ObservableCollection<MeshData> Meshes { get; }
	public ObservableCollection<Vector3> Forces { get; }

	public BoxCollider? BoxCollider { get; set; }
	public Vector3 Velocity { get; set; }

	public WorldObject()
	{
		Id = IdGenerator.GenerateNext();

		Transform = new Transform();
		Meshes = new ObservableCollection<MeshData>();
		Forces = new ObservableCollection<Vector3>();
		Velocity = new Vector3();
	}

	public WorldObject(WorldObject parent)
	{
		Id = IdGenerator.GenerateNext();

		Parent = parent;
		Transform = new Transform();
		Meshes = new ObservableCollection<MeshData>();
		Forces = new ObservableCollection<Vector3>();
		Velocity = new Vector3();
	}

	private void ChildrenChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		// New children was added
		if (e.Action == NotifyCollectionChangedAction.Add)
			foreach (WorldObject addedWorldObject in e.NewItems!)
				addedWorldObject.Parent = this;
		// Old children was removed
		else if (e.Action == NotifyCollectionChangedAction.Remove)
			foreach (WorldObject addedWorldObject in e.OldItems!)
				addedWorldObject.Parent = null;
	}
}