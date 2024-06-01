using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;
using System.Numerics;

namespace GameEngine.Core.API;

public interface IPhysicsEngine
{
	void PhysicsTickPass(float deltaTime); // Passes a physics tick
	int[] GetTouchingColliderIds(int id); // Gets the touching collider ids from a physicsObject id
	void RaycastHitAll(int[] ignoreIds, Vector3 fromPos, Vector3 direction, out List<RaycastHit> hits); // Ray casts from position to direction with ignore list
	public List<int> GetObjectIdsWithinDistance(Vector3 origin, float distance); // Gets all objects within a distance from the received origin


	#region Object management
	void AddPhysicsObject(WorldObject gameObject);
	void RemovePhysicsObject(WorldObject gameObject);
	#endregion
}