using GameEngine.Core.Components;
using System.Collections.ObjectModel;
using System.Numerics;

namespace GameEngine.Components.GameObjectComponents;

internal class GameObject
{
	public int Id { get; }

	public bool RegisteredGraphics { get; set; }
	public bool SyncGraphics
	{
		get => TransformDirty || MeshesDirty || UiDirty;
		set
		{
			TransformDirty = value;
			MeshesDirty = value;
			UiDirty = value;
		}
	}

	public bool RegisteredPhysics { get; set; }
	public bool SyncPhysics
	{
		get => TransformDirty || ForcesDirty;
		set
		{
			TransformDirty = value;
			ForcesDirty = value;
		}
	}

	public bool RegisteredSound { get; set; }
	public bool SyncSound
	{
		get => TransformDirty;
		set
		{
			TransformDirty = value;
		}
	}

	private bool ui;
	public bool UiDirty { get; private set; }
	public bool UI { get => ui; set { ui = value; UiDirty = true; } }

	public bool TransformDirty { get => Transform.Dirty; set => Transform.Dirty = value; }
	public Transform Transform { get; set; }

	public bool MeshesDirty { get; private set; }
	public ObservableCollection<MeshData> Meshes;

	public bool ForcesDirty { get; private set; }
	public ObservableCollection<Vector3> Forces;

	public GameObject(int id)
	{
		Id = id;

		Transform = new Transform();

		Meshes = new ObservableCollection<MeshData>();
		Meshes.CollectionChanged += (s, e) => MeshesDirty = true;

		Forces = new ObservableCollection<Vector3>();
		Forces.CollectionChanged += (s, e) => ForcesDirty = true;
	}
}