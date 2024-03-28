using GameEngine.Core.API;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;
using PhysicsEngine.Components;
using System.Numerics;

namespace PhysicsEngine.Services.Implementations;

internal class PhysicsEngine : IPhysicsEngine
{
    private readonly Dictionary<int, PhysicsObject> physicsObjects = new Dictionary<int, PhysicsObject>();
    private readonly Dictionary<int, PhysicsObject> dynamicColliders = new Dictionary<int, PhysicsObject>();
    private readonly Dictionary<int, PhysicsObject> staticColliders = new Dictionary<int, PhysicsObject>();

    public void PhysicsTickPass(float deltaTime)
    {
        // Apply forces
        foreach (PhysicsObject physicsObject in physicsObjects.Values)
        {
            if (physicsObject.NetForce != Vector3.Zero || physicsObject.Velocity != Vector3.Zero)
            {
                physicsObject.Velocity += physicsObject.NetForce * deltaTime;
                physicsObject.Transform.Position += physicsObject.Velocity * deltaTime;
            }
        }

        // Apply collisions after forces
        foreach (PhysicsObject dynamicCollider in dynamicColliders.Values)
            foreach (PhysicsObject staticCollider in staticColliders.Values)
                if (CollidersOverlap(dynamicCollider, staticCollider, out Vector3 positionDelta))
                    dynamicCollider.Transform.Position += positionDelta;
    }

    public void AddPhysicsObject(WorldObject gameObject)
    {
        if (!physicsObjects.ContainsKey(gameObject.Id))
        {
            PhysicsObject physicsObject = new PhysicsObject(gameObject);
            physicsObjects.Add(gameObject.Id, physicsObject);

            if (gameObject.BoxCollider is not null)
            {
                if (gameObject.BoxCollider.StaticCollider)
                    staticColliders.Add(gameObject.Id, physicsObject);
                else
                    dynamicColliders.Add(gameObject.Id, physicsObject);
            }
        }
    }

    public void RemovePhysicsObject(WorldObject gameObjectData)
    {
        physicsObjects.Remove(gameObjectData.Id);
        staticColliders.Remove(gameObjectData.Id);
        dynamicColliders.Remove(gameObjectData.Id);
    }

    public int[] GetTouchingColliderIds(int id)
    {
        if (!physicsObjects.TryGetValue(id, out PhysicsObject? obj) || obj.BoxCollider is null)
            return Array.Empty<int>();

        Vector3 boundMaxA = obj.Transform.Position + obj.BoxCollider!.Max + Vector3.One;
        Vector3 boundMinA = obj.Transform.Position + obj.BoxCollider!.Min - Vector3.One;

        List<int> collisions = new List<int>();
        foreach (PhysicsObject staticCollider in staticColliders.Values)
        {
            Vector3 boundMaxB = staticCollider.Transform.Position + staticCollider.BoxCollider!.Max;
            Vector3 boundMinB = staticCollider.Transform.Position + staticCollider.BoxCollider!.Min;

            if (boundMaxA.X >= boundMinB.X && boundMinA.X <= boundMaxB.X &&
                   boundMaxA.Y >= boundMinB.Y && boundMinA.Y <= boundMaxB.Y &&
                   boundMaxA.Z >= boundMinB.Z && boundMinA.Z <= boundMaxB.Z)
                collisions.Add(staticCollider.Id);
        }

        return collisions.ToArray();
    }

    private static bool CollidersOverlap(PhysicsObject colliderA, PhysicsObject colliderB, out Vector3 positionDelta)
    {
        Vector3 positionA = colliderA.Transform.Position;
        Vector3 positionB = colliderB.Transform.Position;
        BoxCollider boxA = colliderA.BoxCollider!;
        BoxCollider boxB = colliderB.BoxCollider!;

        Vector3 boundMaxA = positionA + boxA.Max;
        Vector3 boundMinA = positionA + boxA.Min;
        Vector3 boundMaxB = positionB + boxB.Max;
        Vector3 boundMinB = positionB + boxB.Min;

        if (boundMaxA.X < boundMinB.X || boundMinA.X > boundMaxB.X ||
            boundMaxA.Y < boundMinB.Y || boundMinA.Y > boundMaxB.Y ||
            boundMaxA.Z < boundMinB.Z || boundMinA.Z > boundMaxB.Z)
        {
            positionDelta = Vector3.Zero;
            return false;
        }

        float depthX = Math.Min(boundMaxA.X - boundMinB.X, boundMinB.X - boundMaxA.X);
        float depthY = Math.Min(boundMaxA.Y - boundMinB.Y, boundMinB.Y - boundMaxA.Y);
        float depthZ = Math.Min(boundMaxA.Z - boundMinB.Z, boundMinB.Z - boundMaxA.Z);

        // Choose axis with maximum penetration
        if (depthX >= depthY && depthX >= depthZ)
            positionDelta = new Vector3(depthX / 2, 0, 0);
        else if (depthY >= depthX && depthY >= depthZ)
            positionDelta = new Vector3(0, depthY / 2, 0);
        else
            positionDelta = new Vector3(0, 0, depthZ / 2);

        return true; // Overlap detected and position adjusted
    }
}