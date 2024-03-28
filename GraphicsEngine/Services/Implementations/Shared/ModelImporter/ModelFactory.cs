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

    public ModelFactory(ILogger logger, IResourceManager resourceManager, ObjModelImporter objModelImporter)
    {
        this.logger = logger;
        this.resourceManager = resourceManager;
        this.objModelImporter = objModelImporter;
    }

    public bool Create(string modelName, out ModelData modelData)
    {
        modelData = default!;

        if (!resourceManager.LoadResourceLines(modelName, out string[] data))
        {
            logger.LogError("Model {model} Not Found", modelName);
            return false;
        }

        string modelType = modelName.Split('.')[1];
        if (modelType == "obj")
        {
            modelData = objModelImporter.Import(data);
            return true;
        }

        modelData = default!;
        return false;
    }
}