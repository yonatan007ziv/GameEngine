using GameEngine.Core.Components;
using GameEngine.Core.SharedServices.Interfaces;
using GameEngine.Services.Interfaces.Managers;

namespace GameEngine.Services.Implementations.Managers;

internal class GameObjectManager : IGameObjectManager
{
	private readonly IFactory<int, GameObject> gameObjectFactory;
	private readonly List<GameObject> gameObjects = new List<GameObject>();

	private int currentId;

	public GameObjectManager(IFactory<int, GameObject> gameObjectFactory)
	{
		this.gameObjectFactory = gameObjectFactory;
	}

	public GameObject CreateGameObject()
	{
		gameObjectFactory.Create(currentId++, out GameObject gameObject);
		gameObjects.Add(gameObject);
		return gameObject;
	}

	public void RemoveGameObject(GameObject gameObject)
	{
		gameObjects.Remove(gameObject);
	}
}