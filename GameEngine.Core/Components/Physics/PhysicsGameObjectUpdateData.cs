using System.Numerics;

namespace GameEngine.Core.Components.Physics;

public readonly struct PhysicsGameObjectUpdateData
{
	public int Id { get; }
	public TransformData Transform { get; }
	public Vector3 Velocity { get; }

	public PhysicsGameObjectUpdateData(int id, TransformData transform, Vector3 velocity)
	{
		Id = id;
		Transform = transform;
		Velocity = velocity;
	}
}