using OpenGLRenderer.Components;
using OpenGLRenderer.Services.Interfaces.Utils.Factories;

namespace OpenGLRenderer.Services.Implementations.Utils.Factories;

internal class GameObjectFactory : IFactory<GameObject>
{
	private int _id;

	public GameObject Create()
	{
		return new GameObject(_id++);
	}
}