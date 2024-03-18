using GameEngine.Core.Components.Objects;

namespace GameEngine.Core.API;

public interface IPhysicsEngine
{
	void PhysicsTickPass(float deltaTime);

	void AddPhysicsObject(WorldObject gameObject);
	void RemovePhysicsObject(WorldObject gameObject);
	int[] GetTouchingColliderIds(int id);
}