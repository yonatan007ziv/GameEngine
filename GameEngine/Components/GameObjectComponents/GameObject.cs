using GameEngine.Core.Components;
using System.Collections.ObjectModel;

namespace GameEngine.Components.GameObjectComponents;

internal class GameObject
{
	public int Id { get; }

	public bool TransformDirty { get; private set; }
	public Transform Transform { get; }

	public bool MeshesDirty { get; private set; }
	public ObservableCollection<MeshData> Meshes;

	public GameObject(int id)
	{
		Id = id;

		Transform = new Transform(TransformChanged);

		Meshes = new ObservableCollection<MeshData>();
		Meshes.CollectionChanged += (s, e) => MeshesChanged();
	}

	public bool IsDirty()
		=> TransformDirty || MeshesDirty;

	public void ResetDirty()
	{
		TransformDirty = false;
		MeshesDirty = false;
	}

	private void TransformChanged() => TransformDirty = true;
	private void MeshesChanged() => MeshesDirty = true;
}