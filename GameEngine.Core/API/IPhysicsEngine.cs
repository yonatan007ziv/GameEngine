using GameEngine.Core.Components;
using GameEngine.Core.Components.Physics;

namespace GameEngine.Core.API;

public interface IPhysicsEngine
{
	void RegisterPhysicsObject(ref GameObjectData gameObject);
	void UpdatePhysicsObjectForces(ref GameObjectData gameObject);
	List<PhysicsGameObjectUpdateData> PhysicsPass(float deltaTime);
}