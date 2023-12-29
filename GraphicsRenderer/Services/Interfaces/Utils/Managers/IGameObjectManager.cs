using GraphicsRenderer.Components.Shared;

namespace GraphicsRenderer.Services.Interfaces.Utils.Managers;

internal interface IGameObjectManager
{
	public List<GameObject> GameObjects { get; set; }
	GameObject CreateGameObject();
}