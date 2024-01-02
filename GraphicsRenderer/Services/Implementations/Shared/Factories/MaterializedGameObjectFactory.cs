using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;
using Microsoft.Extensions.Logging;

namespace GraphicsRenderer.Services.Implementations.Shared.Factories;

internal class MaterializedGameObjectFactory : IFactory<string, string, string, GameObject>
{
	private readonly ILogger logger;
	private readonly IGameObjectManager gameObjectManager;
	private readonly IFactory<string, IMesh> meshFactory;
	private readonly IFactory<string, string, Material> materialFactory;

	public MaterializedGameObjectFactory(ILogger logger, IGameObjectManager gameObjectManager, IFactory<string, IMesh> meshFactory, IFactory<string, string, Material> materialFactory)
	{
		this.logger = logger;
		this.gameObjectManager = gameObjectManager;
		this.meshFactory = meshFactory;
		this.materialFactory = materialFactory;
	}

	public bool Create(string model, string shader, string texture, out GameObject gameObject)
	{
		if (!meshFactory.Create(model, out IMesh mesh))
		{
			logger.LogError("Error Creating Mesh {modelName}", model);
			// Set Default Mesh
		}

		if (!materialFactory.Create(shader, texture, out Material material))
		{
			logger.LogError("Error Creating Material {shaderName} {materialName}", shader, texture);

			// Set Default Material
			materialFactory.Create("Textured", "MissingTexture.png", out material);
		}

		gameObject = gameObjectManager.CreateGameObject();
		gameObject.Meshes.Add(mesh);
		gameObject.Material = material;
		return true;
	}
}