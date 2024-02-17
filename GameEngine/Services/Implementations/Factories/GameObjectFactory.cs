using GameEngine.Components.Objects;
using GameEngine.Core.SharedServices.Interfaces;

namespace GameEngine.Services.Implementations.Factories;

internal class GameObjectFactory : IFactory<int, WorldObject>
{
	public bool Create(int id, out WorldObject gameObject)
	{
		gameObject = new WorldObject();
		return true;
	}
}