using GameEngine.Core.API;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Physics;
using GameEngine.Core.Extensions;
using PhysicsEngine.Components;
using System.Numerics;

namespace PhysicsEngine.Services.Implementations;

internal class PhysicsEngine : IPhysicsEngine
{
	private readonly Dictionary<int, PhysicsObject> physicsObjects = new Dictionary<int, PhysicsObject>();

	public List<PhysicsGameObjectUpdateData> PhysicsPass(float deltaTime)
	{
		List<PhysicsGameObjectUpdateData> updates = new List<PhysicsGameObjectUpdateData>();

		PhysicsObject[] physicsObjectsArr = physicsObjects.Values.ToArray();
		for (int i = 0; i < physicsObjectsArr.Length; i++)
		{
			PhysicsObject physicsObject = physicsObjectsArr[i];
			if (physicsObject.NetForce != Vector3.Zero || physicsObject.Velocity != Vector3.Zero)
			{
				physicsObject.Velocity += physicsObject.NetForce * deltaTime;
				physicsObject.Transform.Position += physicsObject.Velocity * deltaTime;
				updates.Add(new PhysicsGameObjectUpdateData(physicsObject.Id, physicsObject.Transform.TranslateTransform(), physicsObject.Velocity));
			}
		}
		return updates;
	}

	public void AddPhysicsObject(ref WorldObjectData gameObjectData)
	{
		if (!physicsObjects.ContainsKey(gameObjectData.Id))
			physicsObjects.Add(gameObjectData.Id, new PhysicsObject(gameObjectData.Id, gameObjectData.Transform.TranslateTransform()));
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

			// In physics calculations only position matters
			if (gameObjectData.TransformDirty)
				physicsObject.Transform.Position = gameObjectData.Transform.position;
		}
	}
}