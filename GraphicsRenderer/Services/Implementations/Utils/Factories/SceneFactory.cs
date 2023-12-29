using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Components.Shared.Data;
using GraphicsRenderer.Services.Interfaces.Utils;

namespace GraphicsRenderer.Services.Implementations.Utils.Factories;

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