using GraphicsRenderer.Components.Shared.Data;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;
using Microsoft.Extensions.Logging;

namespace GraphicsRenderer.Services.Implementations.Shared.ModelImporter;

public class ModelFactory : IFactory<string, ModelData>
{
	private readonly ILogger logger;
	private readonly IResourceManager resourceManager;
	private readonly ObjModelImporter objModelImporter;
	private readonly FbxModelImporter fbxModelImporter;

	private readonly Dictionary<string, ModelData> cachedModels = new Dictionary<string, ModelData>();
	
	public ModelFactory(ILogger logger, IResourceManager resourceManager, ObjModelImporter objModelImporter, FbxModelImporter fbxModelImporter)
	{
		this.logger = logger;
		this.resourceManager = resourceManager;
		this.objModelImporter = objModelImporter;
		this.fbxModelImporter = fbxModelImporter;
	}

	public bool Create(string model, out ModelData modelData)
	{
		if (cachedModels.ContainsKey(model))
		{
			modelData = cachedModels[model];
			return true;
		}

		modelData = default!;

		if (!resourceManager.LoadResourceLines(model, out string[] data))
		{
			logger.LogError($"Model \"{model}\" Not Found");
			return false;
		}

		string modelType = model.Split('.')[1];
		if (modelType == "obj")
			modelData = objModelImporter.Import(data);
		else if (modelType == "fbx")
			modelData = fbxModelImporter.Import(data);
		else
			return false;

		cachedModels[model] = modelData;
		return true;
	}
}