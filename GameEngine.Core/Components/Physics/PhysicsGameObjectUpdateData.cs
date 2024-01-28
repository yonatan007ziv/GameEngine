namespace GameEngine.Core.Components.Physics;

public readonly struct PhysicsGameObjectUpdateData
{
	public int Id { get; }
	public TransformData Transform { get; }

	public PhysicsGameObjectUpdateData(int id, TransformData transform)
	{
		Id = id;
		Transform = transform;
	}
}