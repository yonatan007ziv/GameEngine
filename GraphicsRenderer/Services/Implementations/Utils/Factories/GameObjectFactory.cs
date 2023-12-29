using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Services.Interfaces.Utils;

namespace GraphicsRenderer.Services.Implementations.Utils.Factories;

internal class GameObjectFactory : IFactory<GameObject>
{
	private int _id;

	public GameObject Create()
	{
		return new GameObject(_id++);
	}
}