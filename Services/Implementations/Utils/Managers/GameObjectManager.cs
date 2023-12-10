using OpenGLRenderer.Components;
using OpenGLRenderer.Services.Interfaces.Utils.Factories;
using OpenGLRenderer.Services.Interfaces.Utils.Managers;

namespace OpenGLRenderer.Services.Implementations.Utils.Managers;

internal class GameObjectManager : IGameObjectManager
{
	private readonly IFactory<GameObject> gameObjectFactory;

	public GameObjectManager(IFactory<GameObject> gameObjectFactory)
	{
		this.gameObjectFactory = gameObjectFactory;
	}

	public GameObject CreateGameObject()
	{
		// Logic Goes Here

		return gameObjectFactory.Create();
	}
}