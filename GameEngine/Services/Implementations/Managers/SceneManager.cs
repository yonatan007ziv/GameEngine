using GameEngine.Components;
using GameEngine.Services.Interfaces.Managers;

namespace GameEngine.Services.Implementations.Managers;

internal class SceneManager : ISceneManager
{
    private readonly SceneLoader sceneLoader;
    private readonly Dictionary<string, SceneData> scenes = new Dictionary<string, SceneData>();

    public SceneManager(SceneLoader sceneLoader)
    {
        this.sceneLoader = sceneLoader;
    }

    public SceneData LoadScene(string sceneName)
    {
        if (scenes.ContainsKey(sceneName))
            return scenes[sceneName];
        scenes.Add(sceneName, sceneLoader.LoadScene(sceneName));
        return scenes[sceneName];
    }
}