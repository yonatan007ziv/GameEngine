using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Numerics;

namespace GameEngine.Core.Components.Objects;

public abstract class GameObject
{
	public int Id { get; }

	public string Tag { get; protected set; } = "";
	public bool Visible { get; set; } = true;
	public readonly TextData TextData;

	public GameObject? Parent { get; set; }
	public BoxCollider? BoxCollider { get; set; }
	public Vector3 Velocity { get; set; }
	public Transform Transform { get; set; }
	public ObservableCollection<GameObject> Children { get; }
	public ObservableCollection<MeshData> Meshes { get; }
	public ObservableCollection<Vector3> Forces { get; }

	public GameObject(GameObject? parent = null)
	{
		Id = IdGenerator.GenerateNext();

		Parent = parent;
		TextData = new TextData();
		Transform = new Transform();
		Children = new ObservableCollection<GameObject>();
		Meshes = new ObservableCollection<MeshData>();
		Forces = new ObservableCollection<Vector3>();

		Children.CollectionChanged += ChildrenChanged;
	}

	public event Action OnAncestryTreeChanged;

	public (Vector3 position, Vector3 rotation, Vector3 scale) GetRelativeToAncestorTransform()
	{
		if (Parent is null)
			return (Transform.Position, Transform.Rotation, Transform.Scale);
		else
			return ((Parent.Transform.Position + Transform.Position * Parent.Transform.Scale),
				Parent.Transform.Rotation + Transform.Rotation,
				Parent.Transform.Scale * Transform.Scale);
		/*
		// Recursively find the ancestry path to the ancestor
		Stack<GameObject> ancestorPath = FindAncestryPath();

		// Get the parent's size
		Vector3 parentSize = Parent.Transform.Scale;

		// Initialize position, rotation, and scale relative to ancestor
		Vector3 position = Transform.Position / (parentSize / 2f); // Normalize position
		Vector3 rotation = Transform.Rotation;
		Vector3 scale = Transform.Scale / parentSize; // Normalize scale

		// Iterate over the ancestor path
		foreach (GameObject ancestor in ancestorPath)
		{
			// Update position relative to ancestor
			position = (position + ancestor.Transform.Position / (ancestor.Transform.Scale / 2f)) / 2f; // Normalize position

			// Update rotation (assuming this accumulates rotations)
			rotation += ancestor.Transform.Rotation;

			// Update scale relative to ancestor
			scale *= ancestor.Transform.Scale / parentSize; // Normalize scale
		}

		return (position, rotation, scale);
		*/
	}

	private Stack<GameObject> FindAncestryPath(Stack<GameObject>? path = null!)
	{
		if (path is null)
			path = new Stack<GameObject>();

		if (Parent is null)
			return path;

		path.Push(Parent);
		return Parent.FindAncestryPath(path);
	}

	private void ChildrenChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		// TODO: Notify every "blood"-related gameobject that the tree has changed
		OnAncestryTreeChanged?.Invoke();

		// New children was added
		if (e.Action == NotifyCollectionChangedAction.Add)
			foreach (GameObject addedUIObject in e.NewItems!)
				addedUIObject.Parent = this;
		// Old children was removed
		else if (e.Action == NotifyCollectionChangedAction.Remove)
			foreach (GameObject addedUIObject in e.OldItems!)
				addedUIObject.Parent = null;
	}
}