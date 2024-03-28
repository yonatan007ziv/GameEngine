using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;
using System.Numerics;

namespace PhysicsEngine.Components;

internal class PhysicsObject
{
    private readonly WorldObject worldObject;

    public int Id => worldObject.Id;
    public Transform Transform => worldObject.Transform;
    public BoxCollider? BoxCollider => worldObject.BoxCollider;
    public Vector3 Velocity { get => worldObject.Velocity; set => worldObject.Velocity = value; }
    public Vector3 NetForce { get; set; }

    public PhysicsObject(WorldObject worldObject)
    {
        this.worldObject = worldObject;

        worldObject.Forces.CollectionChanged += (s, e) => UpdateForces();
    }

    private void UpdateForces()
    {
        NetForce = Vector3.Zero;
        foreach (var force in worldObject.Forces)
            AddForce(force);
    }

    public void AddForce(Vector3 force)
    {
        NetForce += force;
    }
}