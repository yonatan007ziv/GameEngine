using GameEngine.Core.Components;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Numerics;

namespace GameEngine.Components;

public class GameObject : IDisposable
{
	public int Id { get; }

	public bool IsUI { get; protected set; }

	public bool SyncGraphics { get; set; } = true;
	public bool SyncPhysics { get; set; }
	public bool SyncSound { get; set; }

	public bool TransformDirty { get; private set; }
	private Transform _transform;
	public Transform Transform
	{
		get => _transform;
		set
		{
			_transform.PropertyChanged -= TransformChanged;
			_transform = value;
			_transform.PropertyChanged += TransformChanged;
		}
	}

	public bool MeshesDirty { get; private set; }
	private ObservableCollection<MeshData> _meshes;
	public ObservableCollection<MeshData> Meshes
	{
		get => _meshes;
		set
		{
			_meshes.CollectionChanged -= ImpulseVelocitiesChanged;
			_meshes = value;
			_meshes.CollectionChanged += ImpulseVelocitiesChanged;
		}
	}

	public bool ForcesDirty { get; private set; }
	private ObservableCollection<Vector3> _forces;
	public ObservableCollection<Vector3> Forces
	{
		get => _forces;
		set
		{
			_forces.CollectionChanged -= ImpulseVelocitiesChanged;
			_forces = value;
			_forces.CollectionChanged += ImpulseVelocitiesChanged;
		}
	}

	public bool ImpulseVelocitiesDirty { get; private set; }
	private ObservableCollection<Vector3> _impulseVelocities;
	public ObservableCollection<Vector3> ImpulseVelocities
	{
		get => _impulseVelocities;
		set
		{
			_impulseVelocities.CollectionChanged -= ImpulseVelocitiesChanged;
			_impulseVelocities = value;
			_impulseVelocities.CollectionChanged += ImpulseVelocitiesChanged;
		}
	}

	public GameObject()
	{
		Id = IdGenerator.GenerateNext();

		_transform = new Transform();
		_transform.PropertyChanged += TransformChanged;

		_meshes = new ObservableCollection<MeshData>();
		_meshes.CollectionChanged += MeshesChanged;

		_forces = new ObservableCollection<Vector3>();
		_forces.CollectionChanged += ForcesChanged;

		_impulseVelocities = new ObservableCollection<Vector3>();
		_impulseVelocities.CollectionChanged += ImpulseVelocitiesChanged;
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
		ForcesDirty = false;
		ImpulseVelocitiesDirty = false;
		SyncPhysics = false;
		SyncSound = false;
	}

	public void Dispose()
	{
		Meshes.Clear();
		Forces.Clear();
		ImpulseVelocities.Clear();
	}
}