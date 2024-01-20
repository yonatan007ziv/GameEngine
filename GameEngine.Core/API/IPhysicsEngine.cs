using GameEngine.Core.Components;

namespace GameEngine.Core.API;

public interface IPhysicsEngine
{
	void UpdateGameObject(ref GameObjectData gameObject);
	void PhysicsPass(float deltaTime);
}