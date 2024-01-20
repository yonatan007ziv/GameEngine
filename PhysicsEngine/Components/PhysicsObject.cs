using GameEngine.Core.Components;
using System.Numerics;

namespace PhysicsEngine.Components;

internal class PhysicsObject
{
	public int Id { get; }
	public Transform Transform { get; set; }
	public float Gravity { get; set; }

	public Vector3 Velocity { get; set; }

	public PhysicsObject(int id, Transform transform, float gravity)
	{
		Id = id;
		Transform = transform;
		Gravity = gravity;

		Velocity = Vector3.Zero;
	}
}