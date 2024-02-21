using System.Numerics;

namespace GameEngine.Core.Components;

public readonly struct WorldObjectData
{
	public int Id { get; }

	public bool TransformDirty { get; }
	public TransformData Transform { get; }

	public bool BoxColliderDirty { get; }
	public BoxColliderData? BoxCollider { get; }

	public bool MeshesDirty { get; }
	public List<MeshData> Meshes { get; }

	public bool ForcesDirty { get; }
	public List<Vector3> Forces { get; }

	public bool ImpulseVelocitiesDirty { get; }
	public List<Vector3> ImpulseVelocities { get; }

	public WorldObjectData(int id,
		TransformData transform, bool transformDirty,
		BoxColliderData? boxCollider, bool boxColliderDirty,
		List<MeshData> meshes, bool meshesDirty,
		List<Vector3> forces, bool forcesDirty,
		List<Vector3> impulseVelocities, bool impulseVelocitiesDirty)
	{
		Id = id;

		Transform = transform;
		TransformDirty = transformDirty;

		BoxCollider = boxCollider;
		BoxColliderDirty = boxColliderDirty;

		Meshes = meshes;
		MeshesDirty = meshesDirty;

		Forces = forces;
		ForcesDirty = forcesDirty;

		ImpulseVelocities = impulseVelocities;
		ImpulseVelocitiesDirty = impulseVelocitiesDirty;
	}
}