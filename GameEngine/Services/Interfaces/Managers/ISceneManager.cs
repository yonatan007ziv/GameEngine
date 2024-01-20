using GameEngine.Components;

namespace GameEngine.Services.Interfaces.Managers;

internal interface ISceneManager
{
	SceneData LoadScene(string sceneName);
}