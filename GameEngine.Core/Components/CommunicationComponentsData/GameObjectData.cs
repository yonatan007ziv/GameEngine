namespace GameEngine.Core.Components.CommunicationComponentsData;

public readonly struct GameObjectData
{
	public int Id { get; }

	public TransformData Transform { get; }
	public bool TransformDirty { get; }

	public List<MeshData> Meshes { get; }
	public bool MeshesDirty { get; }

	public GameObjectData(int id, TransformData transform, bool transformDirty, List<MeshData> meshes, bool meshesDirty)
	{
		Id = id;

		Transform = transform;
		TransformDirty = transformDirty;

		Meshes = meshes;
		MeshesDirty = meshesDirty;
	}
}