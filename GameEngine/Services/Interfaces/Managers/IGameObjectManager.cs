using GameEngine.Components.Objects;

namespace GameEngine.Services.Interfaces.Managers;

internal interface IGameObjectManager
{
	List<WorldObject> GameObjects { get; }

	WorldObject CreateGameObject();
	void RemoveGameObject(WorldObject gameObject);
}