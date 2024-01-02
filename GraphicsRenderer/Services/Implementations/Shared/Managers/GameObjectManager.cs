using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;

namespace GraphicsRenderer.Services.Implementations.Shared.Managers;

public class GameObjectManager : IGameObjectManager
{
	private readonly IFactory<IGameObjectManager, GameObject> gameObjectFactory;

	public List<GameObject> GameObjects { get; set; }

	public GameObjectManager(IFactory<IGameObjectManager, GameObject> gameObjectFactory)
	{
		this.gameObjectFactory = gameObjectFactory;
		GameObjects = new List<GameObject>();
	}

	public GameObject CreateGameObject()
	{
		gameObjectFactory.Create(this, out GameObject current);
		GameObjects.Add(current);
		return current;
	}
}