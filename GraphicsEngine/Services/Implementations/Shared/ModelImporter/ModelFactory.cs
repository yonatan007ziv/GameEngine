using GameEngine.Core.SharedServices.Interfaces;
using GameEngine.Core.SharedServices.Interfaces.Utils.Managers;
using GraphicsEngine.Components.Shared.Data;
using Microsoft.Extensions.Logging;

namespace GraphicsEngine.Services.Implementations.Shared.ModelImporter;

internal class ModelFactory : IFactory<string, ModelData>
{
	private readonly ILogger logger;
	private readonly IResourceManager resourceManager;
	private readonly ObjModelImporter objModelImporter;

	private readonly Dictionary<string, ModelData> cachedModels = new Dictionary<string, ModelData>();

	public ModelFactory(ILogger logger, IResourceManager resourceManager, ObjModelImporter objModelImporter)
	{
		this.logger = logger;
		this.resourceManager = resourceManager;
		this.objModelImporter = objModelImporter;
	}

	public bool Create(string modelName, out ModelData modelData)
	{
		if (cachedModels.ContainsKey(modelName))
		{
			modelData = cachedModels[modelName];
			return true;
		}

		modelData = default!;

		if (!resourceManager.LoadResourceLines(modelName, out string[] data))
		{
			logger.LogError("Model {model} Not Found", modelName);
			return false;
		}

		string modelType = modelName.Split('.')[1];
		if (modelType == "obj")
			modelData = objModelImporter.Import(data);
		else
			return false;

		cachedModels[modelName] = modelData;
		return true;
	}
}