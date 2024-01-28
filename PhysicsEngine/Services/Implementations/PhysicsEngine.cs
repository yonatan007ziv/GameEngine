using GameEngine.Core.API;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Physics;
using GameEngine.Core.Extensions;
using PhysicsEngine.Components;
using System.Numerics;

namespace PhysicsEngine.Services.Implementations;

internal class PhysicsEngine : IPhysicsEngine
{
	private readonly List<PhysicsObject> physicsObjects = new List<PhysicsObject>();

	public List<PhysicsGameObjectUpdateData> PhysicsPass(float deltaTime)
	{
		List<PhysicsGameObjectUpdateData> updates = new List<PhysicsGameObjectUpdateData>();
		for (int i = 0; i < physicsObjects.Count; i++)
		{
			PhysicsObject physicsObject = physicsObjects[i];
			if (physicsObject.NetForce != Vector3.Zero)
			{
				physicsObject.Velocity += physicsObject.NetForce * deltaTime;
				physicsObject.Transform.Position += physicsObject.Velocity * deltaTime;
				updates.Add(new PhysicsGameObjectUpdateData(physicsObject.Id, physicsObject.Transform.TranslateTransform()));
			}
		}
		return updates;
	}

	public void RegisterPhysicsObject(ref GameObjectData gameObjectData)
	{
		int updateId = gameObjectData.Id;
		PhysicsObject? gameObject = physicsObjects.Find(obj => obj.Id == updateId);

		if (gameObject is null) // Register object
			physicsObjects.Add(new PhysicsObject(gameObjectData.Id, gameObjectData.Transform.TranslateTransform()));
	}

	public void UpdatePhysicsObjectForces(ref GameObjectData gameObjectData)
	{
		int updateId = gameObjectData.Id;
		PhysicsObject? physicsObject = physicsObjects.Find(obj => obj.Id == updateId);

		if (physicsObject is not null)
		{
			if (gameObjectData.ForcesDirty)
			{
				physicsObject.NetForce = Vector3.Zero;
				foreach (Vector3 force in gameObjectData.Forces)
					physicsObject.AddForce(force);
			}
		}
	}
}