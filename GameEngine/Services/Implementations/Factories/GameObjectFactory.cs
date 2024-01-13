using GameEngine.Components.GameObjectComponents;
using GameEngine.Core.SharedServices.Interfaces;

namespace GameEngine.Services.Implementations.Factories;

internal class GameObjectFactory : IFactory<int, GameObject>
{
	public bool Create(int id, out GameObject gameObject)
	{
		gameObject = new GameObject(id);
		return true;
	}
}