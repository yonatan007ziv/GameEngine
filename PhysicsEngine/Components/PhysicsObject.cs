using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;
using System.Numerics;

namespace PhysicsEngine.Components;

internal class PhysicsObject
{
	private readonly GameObject gameObject;

	public int Id => gameObject.Id;
	public Transform Transform => gameObject.Transform;
	public BoxCollider? BoxCollider => gameObject.BoxCollider;
	public Vector3 Velocity { get => gameObject.Velocity; set => gameObject.Velocity = value; }
	public Vector3 NetForce { get; set; }

	public PhysicsObject(GameObject gameObject)
	{
		this.gameObject = gameObject;

		gameObject.Forces.CollectionChanged += (s, e) => UpdateForces();
		UpdateForces();
	}

	private void UpdateForces()
	{
		NetForce = Vector3.Zero;
		foreach (var force in gameObject.Forces)
			NetForce += force;
	}
}