using GraphicsRenderer.Services.Interfaces.Utils.Managers;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Components.Shared;

namespace GraphicsRenderer.Services.Implementations.Utils.Managers;

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