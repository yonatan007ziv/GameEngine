using OpenGLRenderer.Models;
using OpenGLRenderer.Services.Interfaces.Utils.Factories;
using OpenGLRenderer.Services.Interfaces.Utils.Managers;

namespace OpenGLRenderer.Services.Implementations.Utils.Managers;

internal class SceneManager : ISceneManager
{
	private readonly IFactory<Scene> sceneFactory;
	public Scene CurrentScene { get; private set; }

	public SceneManager(IFactory<Scene> sceneFactory)
	{
		this.sceneFactory = sceneFactory;
	}

	public void LoadScene(string path)
	{
		CurrentScene = sceneFactory.Create();
		//Scene scene = sceneFactory.Create();
		// scene.Load(path);
	}
}