using GameEngine.Core.API;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Physics;
using GameEngine.Core.Extensions;
using PhysicsEngine.Components;
using System.Numerics;

namespace PhysicsEngine;

internal class PhysicsEngine : IPhysicsEngine
{
	private readonly Dictionary<int, PhysicsObject> physicsObjects = new Dictionary<int, PhysicsObject>();
	private readonly Dictionary<int, PhysicsObject> dynamicColliders = new Dictionary<int, PhysicsObject>();
	private readonly Dictionary<int, PhysicsObject> staticColliders = new Dictionary<int, PhysicsObject>();

	public PhysicsGameObjectUpdateData[] PhysicsPass(float deltaTime)
	{
		Dictionary<int, PhysicsGameObjectUpdateData> physicsUpdates = new Dictionary<int, PhysicsGameObjectUpdateData>();

		PhysicsObject[] physicsObjects = this.physicsObjects.Values.ToArray();
		PhysicsObject[] dynamicColliders = this.dynamicColliders.Values.ToArray();
		PhysicsObject[] staticColliders = this.staticColliders.Values.ToArray();

		// Apply forces
		foreach (PhysicsObject physicsObject in physicsObjects)
		{
			if (physicsObject.NetForce != Vector3.Zero || physicsObject.Velocity != Vector3.Zero)
			{
				physicsObject.Velocity += physicsObject.NetForce * deltaTime;
				physicsObject.Transform.Position += physicsObject.Velocity * deltaTime;
				physicsUpdates[physicsObject.Id] = new PhysicsGameObjectUpdateData(physicsObject.Id, physicsObject.Transform.TranslateTransform(), physicsObject.Velocity);
			}
		}

		// Apply collisions after forces
		foreach (PhysicsObject dynamicCollider in dynamicColliders)
		{
			bool collided = false;
			foreach (PhysicsObject staticCollider in staticColliders)
			{
				if (CollidersOverlap(dynamicCollider, staticCollider, out Vector3 positionDelta))
				{
					dynamicCollider.Transform.Position += positionDelta;
					collided = true;
				}
			}
			if (collided)
				physicsUpdates[dynamicCollider.Id] = new PhysicsGameObjectUpdateData(dynamicCollider.Id, dynamicCollider.Transform.TranslateTransform(), dynamicCollider.Velocity);
		}

		return physicsUpdates.Values.ToArray();
	}

	public void AddPhysicsObject(ref WorldObjectData gameObjectData)
	{
		if (!physicsObjects.ContainsKey(gameObjectData.Id))
			physicsObjects.Add(gameObjectData.Id, new PhysicsObject(gameObjectData.Id, gameObjectData.Transform.TranslateTransform(), gameObjectData.BoxCollider));
	}

	public void RemovePhysicsObject(ref WorldObjectData gameObjectData)
	{
		if (physicsObjects.ContainsKey(gameObjectData.Id))
			physicsObjects.Remove(gameObjectData.Id);
	}

	public void UpdatePhysicsObject(ref WorldObjectData gameObjectData)
	{
		int updateId = gameObjectData.Id;
		PhysicsObject? physicsObject = physicsObjects.ContainsKey(updateId) ? physicsObjects[updateId] : null;

		if (physicsObject is not null)
		{
			if (gameObjectData.ForcesDirty)
			{
				physicsObject.NetForce = Vector3.Zero;
				foreach (Vector3 force in gameObjectData.Forces)
					physicsObject.AddForce(force);
			}

			if (gameObjectData.ImpulseVelocitiesDirty)
			{
				Vector3 sum = Vector3.Zero;
				foreach (Vector3 vel in gameObjectData.ImpulseVelocities)
					sum += vel;
				physicsObject.Velocity = sum;
			}

			if (gameObjectData.TransformDirty)
			{
				physicsObject.Transform.Position = gameObjectData.Transform.position;
				physicsObject.Transform.Scale = gameObjectData.Transform.scale;
			}

			if (gameObjectData.BoxColliderDirty)
			{
				physicsObject.BoxCollider = gameObjectData.BoxCollider;
				if (gameObjectData.BoxCollider.HasValue)
					if (gameObjectData.BoxCollider.Value.StaticCollider)
						staticColliders.Add(physicsObject.Id, physicsObject);
					else
						dynamicColliders.Add(physicsObject.Id, physicsObject);
				else
				{
					staticColliders.Remove(physicsObject.Id);
					dynamicColliders.Remove(physicsObject.Id);
				}
			}
		}
	}

	public bool CollidersOverlap(PhysicsObject colliderA, PhysicsObject colliderB, out Vector3 positionDelta)
	{
		Vector3 positionA = colliderA.Transform.Position;
		Vector3 positionB = colliderB.Transform.Position;
		BoxColliderData boxA = colliderA.BoxCollider!.Value;
		BoxColliderData boxB = colliderB.BoxCollider!.Value;

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

	public int[] GetTouchingColliderIds(int id)
	{
		if (!physicsObjects.TryGetValue(id, out PhysicsObject? obj) || !obj!.BoxCollider.HasValue)
			return Array.Empty<int>();

		// Vector3.One / 10 is the tolerance
		Vector3 boundMaxA = obj.Transform.Position + obj.BoxCollider.Value.Max + Vector3.One;
		Vector3 boundMinA = obj.Transform.Position + obj.BoxCollider.Value.Min - Vector3.One;

		List<int> collisions = new List<int>();
		foreach (PhysicsObject staticCollider in staticColliders.Values)
		{
			Vector3 boundMaxB = staticCollider.Transform.Position + staticCollider.BoxCollider!.Value.Max;
			Vector3 boundMinB = staticCollider.Transform.Position + staticCollider.BoxCollider!.Value.Min;

			if ((boundMaxA.X >= boundMinB.X && boundMinA.X <= boundMaxB.X) &&
				   (boundMaxA.Y >= boundMinB.Y && boundMinA.Y <= boundMaxB.Y) &&
				   (boundMaxA.Z >= boundMinB.Z && boundMinA.Z <= boundMaxB.Z))
				collisions.Add(staticCollider.Id);
		}

		return collisions.ToArray();
	}
}