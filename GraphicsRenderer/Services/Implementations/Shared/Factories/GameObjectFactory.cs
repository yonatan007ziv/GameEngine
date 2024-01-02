using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;

namespace GraphicsRenderer.Services.Implementations.Shared.Factories;

public class GameObjectFactory : IFactory<IGameObjectManager, GameObject>
{
	private int currentId;

	public bool Create(IGameObjectManager gameObjectManager, out GameObject gameObject)
	{
		gameObject = new GameObject(gameObjectManager, currentId++);
		return true;
	}
}