using OpenGLRenderer.Components;

namespace OpenGLRenderer.Models;

internal class Scene
{
	private List<GameObject> gameObjects;

	private Camera sceneCamera; // temp obviously

	public Scene(SceneData sceneData)
	{
		gameObjects = new List<GameObject>();
		// ImportData(sceneData);
	}

	//private void ImportData(SceneData sceneData)
	//{

	//}

	public void RenderScene(float deltaTime = 0) // deltaTime usage?
	{
		foreach (GameObject gameObject in gameObjects)
			gameObject.Render(sceneCamera);
	}

	public void UpdateScene(float deltaTime = 0) // deltaTime usage?
	{
		// collisions and such
	}
}