using System.Numerics;

namespace GameEngine.Core.Components;

public readonly struct GameObjectData
{
	public int Id { get; }

	public bool UI { get; }
	public bool TransformDirty { get; }
	public TransformData Transform { get; }

	public bool MeshesDirty { get; }
	public List<MeshData> Meshes { get; }

	public bool ForcesDirty { get; }
	public List<Vector3> Forces { get; }

	public GameObjectData(int id, bool isUI,
		TransformData transform, bool transformDirty,
		List<MeshData> meshes, bool meshesDirty,
		List<Vector3> forces, bool forcesDirty)
	{
		Id = id;

		UI = isUI;

		Transform = transform;
		TransformDirty = transformDirty;

		Meshes = meshes;
		MeshesDirty = meshesDirty;

		Forces = forces;
		ForcesDirty = forcesDirty;
	}
}