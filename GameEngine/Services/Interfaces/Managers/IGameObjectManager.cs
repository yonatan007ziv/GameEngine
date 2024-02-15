using GameEngine.Components;

namespace GameEngine.Services.Interfaces.Managers;

internal interface IGameObjectManager
{
	List<GameObject> GameObjects { get; }

	GameObject CreateGameObject();
	void RemoveGameObject(GameObject gameObject);
}