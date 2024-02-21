using GameEngine.Core.Components;
using GameEngine.Core.Components.Physics;

namespace GameEngine.Core.API;

public interface IPhysicsEngine
{
	void AddPhysicsObject(ref WorldObjectData gameObject);
	void RemovePhysicsObject(ref WorldObjectData gameObject);

	void UpdatePhysicsObject(ref WorldObjectData gameObject);
	PhysicsGameObjectUpdateData[] PhysicsPass(float deltaTime);

	int[] GetTouchingColliderIds(int id);
}