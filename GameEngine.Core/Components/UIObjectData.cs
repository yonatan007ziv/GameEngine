namespace GameEngine.Core.Components;

public readonly struct UIObjectData
{
	public int Id { get; }

	public bool TransformDirty { get; }
	public TransformData Transform { get; }

	public bool MeshesDirty { get; }
	public List<MeshData> Meshes { get; }

	public UIObjectData(int id,
		TransformData transform, bool transformDirty,
		List<MeshData> meshes, bool meshesDirty)
	{
		Id = id;

		Transform = transform;
		TransformDirty = transformDirty;

		Meshes = meshes;
		MeshesDirty = meshesDirty;
	}
}