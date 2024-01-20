using GameEngine.Core.Components;
using System.Collections.ObjectModel;

namespace GameEngine.Components.GameObjectComponents;

internal class GameObject
{
	public int Id { get; }
	public bool Dirty
	{
		get => Transform.TransformDirty || MeshesDirty;
		set
		{
			Transform.TransformDirty = value;
			MeshesDirty = value;
		}
	}

	public Transform Transform { get; set; }

	public bool MeshesDirty { get; private set; }
	public ObservableCollection<MeshData> Meshes;

	public GameObject(int id)
	{
		Id = id;

		Transform = new Transform();

		Meshes = new ObservableCollection<MeshData>();
		Meshes.CollectionChanged += (s, e) => MeshesDirty = true;

		Dirty = true;
	}
}