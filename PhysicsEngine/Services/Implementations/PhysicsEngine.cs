using GameEngine.Core.API;
using GameEngine.Core.Components;
using PhysicsEngine.Components;

namespace PhysicsEngine.Services.Implementations;

internal class PhysicsEngine : IPhysicsEngine
{
	private readonly List<PhysicsObject> physicsObjects = new List<PhysicsObject>();

	bool first = true;
	public void PhysicsPass(float deltaTime)
	{
		for (int i = 0; i < physicsObjects.Count; i++)
		{
			PhysicsObject physicsObject = physicsObjects[i];
			physicsObject.Velocity += new System.Numerics.Vector3(0, physicsObject.Gravity * deltaTime, 0);
			physicsObject.Transform.Position += physicsObject.Velocity * deltaTime;

			if (first)
			{
				physicsObject.Velocity = new System.Numerics.Vector3(5, 25, 0);
				first = false;
			}
		}
	}

	public void UpdateGameObject(ref GameObjectData gameObjectData)
	{
		int updateId = gameObjectData.Id;
		PhysicsObject? gameObject = physicsObjects.Find(obj => obj.Id == updateId);

		if (gameObject is null) // Register object
			physicsObjects.Add(new PhysicsObject(gameObjectData.Id, gameObjectData.Transform, -10));
		else if (gameObjectData.TransformDirty) // Update object
			gameObject.Transform = gameObjectData.Transform;
	}
}