using GameEngine.Core.Components;
using GameEngine.Core.Components.Physics;

namespace GameEngine.Core.API;

public interface IPhysicsEngine
{
	void AddPhysicsObject(ref GameObjectData gameObject);
	void RemovePhysicsObject(ref GameObjectData gameObject);

	void UpdatePhysicsObject(ref GameObjectData gameObject);
	List<PhysicsGameObjectUpdateData> PhysicsPass(float deltaTime);
}