namespace GameEngine.Core.Components;

public readonly struct GameObjectData
{
	public int Id { get; }

	public bool TransformDirty { get; }
	public Transform Transform { get; }

	public bool MeshesDirty { get; }
	public List<MeshData> Meshes { get; }

	public GameObjectData(int id, Transform transform, List<MeshData> meshes)
	{
		Id = id;

		Transform = transform;
		TransformDirty = false;

		Meshes = meshes;
		MeshesDirty = false;
	}
}