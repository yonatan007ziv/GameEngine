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

	// Relative to ancestry tree
	public bool UseRelativePosition { get; set; } = true;
	public bool UseRelativeRotation { get; set; } = true;
	public bool UseRelativeScale { get; set; } = true;
	private Vector3 relativePosition;
	private Vector3 relativeRotation;
	private Vector3 relativeScale;

	private GameObject? _parent;
	public GameObject? Parent { get => _parent; set { _parent?.Children.Remove(this); _parent = value; ChildTreeChanged(); } }
	public ObservableCollection<GameObject> Children { get; }

	public BoxCollider? BoxCollider { get; set; }
	public Vector3 Velocity { get; set; }
	public Transform Transform { get; set; }
	public ObservableCollection<MeshData> Meshes { get; }
	public ObservableCollection<Vector3> Forces { get; }

	public GameObject(GameObject? parent = null)
	{
		Id = IdGenerator.GenerateNext();
		TextData = new TextData();
		Transform = new Transform();
		Children = new ObservableCollection<GameObject>();
		Meshes = new ObservableCollection<MeshData>();
		Forces = new ObservableCollection<Vector3>();

		Parent = parent;
		Children.CollectionChanged += ChildrenChanged;
		Transform.PropertyChanged += (s, e) => ChildTreeChanged();
	}

	public (Vector3 position, Vector3 rotation, Vector3 scale) GetRelativeToAncestorTransform()
		=> (
			UseRelativePosition ? relativePosition : Transform.Position,
			UseRelativeRotation ? relativeRotation : Transform.Rotation,
			UseRelativeScale ? relativeScale : Transform.Scale
			);

	private void RecalculateRelativeTransform()
	{
		// Recursively find the ancestry path to the ancestor
		Stack<GameObject> ancestorPath = FindAncestryPath();

		Vector3 position = Vector3.Zero;
		Vector3 rotation = Vector3.Zero;
		Vector3 scale = Vector3.One;

		// Iterate over the ancestor path
		while (ancestorPath.Count > 0)
		{
			GameObject ancestor = ancestorPath.Pop();
			position += ancestor.Transform.Position * scale;
			rotation += ancestor.Transform.Rotation;
			scale *= ancestor.Transform.Scale;
		}

		relativePosition = position + Transform.Position * scale;
		relativeRotation = rotation + Transform.Rotation;
		relativeScale = scale * Transform.Scale;
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

	private void ChildTreeChanged()
	{
		RecalculateRelativeTransform();
		// Notify every "blood"-related gameobject that the sub-tree has changed
		foreach (GameObject child in Children)
			child.ChildTreeChanged();
	}

	private void ChildrenChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		ChildTreeChanged();

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