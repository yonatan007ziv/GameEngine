using GameEngine.Core.Components;
using System.Numerics;

namespace PhysicsEngine.Components;

internal class PhysicsObject
{
	public int Id { get; }
	public Transform Transform { get; set; }
	public Vector3 Velocity { get; set; }
	public Vector3 NetForce { get; set; }

	public PhysicsObject(int id, Transform transform)
	{
		Id = id;
		Transform = transform;

		Velocity = Vector3.Zero;
	}

	public void AddForce(Vector3 force)
	{
		NetForce += force;
	}
}