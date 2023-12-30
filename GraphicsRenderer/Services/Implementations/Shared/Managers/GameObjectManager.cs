using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;

namespace GraphicsRenderer.Services.Implementations.Shared.Managers;

internal class GameObjectManager : IGameObjectManager
{
	private readonly IFactory<GameObject> gameObjectFactory;

	public List<GameObject> GameObjects { get; set; }

	public GameObjectManager(IFactory<GameObject> gameObjectFactory)
	{
		this.gameObjectFactory = gameObjectFactory;
		GameObjects = new List<GameObject>();
	}

	public GameObject CreateGameObject()
	{
		GameObject current = gameObjectFactory.Create();
		GameObjects.Add(current);
		return current;
	}
}