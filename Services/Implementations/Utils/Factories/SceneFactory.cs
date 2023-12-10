using OpenGLRenderer.Models;
using OpenGLRenderer.Services.Interfaces.Utils.Factories;

namespace OpenGLRenderer.Services.Implementations.Utils.Factories;

internal class SceneFactory : IFactory<Scene>
{
	public SceneFactory()
	{

	}

	public Scene Create()
	{
		SceneData sceneData = new SceneData();
		return new Scene(sceneData);
	}
}