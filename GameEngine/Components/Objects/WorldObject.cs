using GameEngine.Core.Components;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Numerics;

namespace GameEngine.Components.Objects;

public class WorldObject
{
	public int Id { get; }
	public string Tag { get; protected set; } = "";

	public readonly List<WorldComponent> components = new List<WorldComponent>();

	public bool SyncGraphics { get; set; } = true;
	public bool SyncPhysics { get; set; }
	public bool SyncSound { get; set; }

	public bool TransformDirty { get; set; }
	public Transform Transform { get; }

	public bool BoxColliderDirty { get; set; }
	private BoxColliderData? _boxCollider;
	public BoxColliderData? BoxCollider { get => _boxCollider; set { _boxCollider = value; BoxColliderDirty = true; SyncPhysics = true; } }

	public bool MeshesDirty { get; set; }
	public ObservableCollection<MeshData> Meshes { get; }

	public bool ForcesDirty { get; set; }
	public ObservableCollection<Vector3> Forces { get; }

	public bool ImpulseVelocitiesDirty { get; set; }
	public ObservableCollection<Vector3> ImpulseVelocities { get; }

	public WorldObject()
	{
		Id = IdGenerator.GenerateNext();

		Transform = new Transform();
		Transform.PropertyChanged += TransformChanged;

		Meshes = new ObservableCollection<MeshData>();
		Meshes.CollectionChanged += MeshesChanged;

		Forces = new ObservableCollection<Vector3>();
		Forces.CollectionChanged += ForcesChanged;

		ImpulseVelocities = new ObservableCollection<Vector3>();
		ImpulseVelocities.CollectionChanged += ImpulseVelocitiesChanged;
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

	private void ForcesChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		ForcesDirty = true;
		SyncPhysics = true;
	}

	private void ImpulseVelocitiesChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		ImpulseVelocitiesDirty = true;
		SyncPhysics = true;
	}

	internal void ResetPhysicsSoundDirty()
	{
		BoxColliderDirty = false;
		ForcesDirty = false;
		ImpulseVelocitiesDirty = false;
		SyncPhysics = false;
		SyncSound = false;
	}
}