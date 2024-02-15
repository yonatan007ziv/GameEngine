using GameEngine.Components;
using GameEngine.Core.SharedServices.Interfaces;

namespace GameEngine.Services.Implementations.Factories;

internal class GameObjectFactory : IFactory<int, GameObject>
{
	public bool Create(int id, out GameObject gameObject)
	{
		gameObject = new GameObject();
		return true;
	}
}