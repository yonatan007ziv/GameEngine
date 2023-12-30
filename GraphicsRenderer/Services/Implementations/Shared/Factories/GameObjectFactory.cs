using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;

namespace GraphicsRenderer.Services.Implementations.Shared.Factories;

internal class GameObjectFactory : IFactory<GameObject>
{
	private readonly IShaderManager shaderManager;

	private int currentId;

	public GameObjectFactory(IShaderManager shaderManager)
	{
		this.shaderManager = shaderManager;
	}

	public GameObject Create()
	{
		return new GameObject(shaderManager.GetShader("Default"), currentId++);
	}
}