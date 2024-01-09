using GameEngine.Core.Components;

namespace GameEngine.Services.Interfaces.Managers;

internal interface IGameObjectManager
{
	GameObject CreateGameObject();
	void RemoveGameObject(GameObject gameObject);
}