using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Numerics;

namespace GameEngine.Core.Components.Objects;

public abstract class GameObject
{
	public int Id { get; }

	public string Tag { get; set; } = "";
	public bool Visible { get; set; } = true;
	public readonly TextData TextData;

	// Invoked when the object gets added to a children list of another object
	public Action? OnAddedToChildren;
	public Action? OnRemovedFromChildren;

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

	// Recursive using ChildTreeChanged, do not use alone
	// Relies on parent having the correct relative transform according to its entire ancestry
	private void RecalculateRelativeTransform()
	{
		if (Parent is null)
		{
			relativePosition = Transform.Position;
			relativeRotation = Transform.Rotation;
			relativeScale = Transform.Scale;
			return;
		}

		(Vector3 position, Vector3 rotation, Vector3 scale) = Parent.GetRelativeToAncestorTransform();

		relativePosition = position + Transform.Position * scale;
		relativeRotation = rotation + Transform.Rotation;
		relativeScale = scale * Transform.Scale;
	}

	private void ChildTreeChanged()
	{
		RecalculateRelativeTransform();
		// Notify every "blood"-related gameobject that the sub-tree has changed
		for (int i = 0; i < Children.Count; i++)
			Children[i].ChildTreeChanged();
	}

	private void ChildrenChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		ChildTreeChanged();

		// New children was added
		if (e.Action == NotifyCollectionChangedAction.Add)
			foreach (GameObject addedUIObject in e.NewItems!)
			{
				addedUIObject.Parent = this;
				addedUIObject.OnAddedToChildren?.Invoke();
			}
		// Old children was removed
		else if (e.Action == NotifyCollectionChangedAction.Remove)
			foreach (GameObject addedUIObject in e.OldItems!)
			{
				addedUIObject.Parent = null;
				addedUIObject.OnRemovedFromChildren?.Invoke();
			}
	}
}