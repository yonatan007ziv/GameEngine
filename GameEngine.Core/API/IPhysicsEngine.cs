using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;
using System.Numerics;

namespace GameEngine.Core.API;

public interface IPhysicsEngine
{
	void PhysicsTickPass(float deltaTime);

	void AddPhysicsObject(WorldObject gameObject);
	void RemovePhysicsObject(WorldObject gameObject);
	int[] GetTouchingColliderIds(int id);
	void RaycastHitAll(int[] ignoreIds, Vector3 fromPos, Vector3 direction, out List<RaycastHit> hits);
	public List<int> GetObjectIdsWithinDistance(Vector3 origin, float distance);
}