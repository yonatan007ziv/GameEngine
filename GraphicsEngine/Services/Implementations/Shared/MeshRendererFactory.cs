using GameEngine.Core.SharedServices.Interfaces;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Components.Shared.Data;
using GraphicsEngine.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphicsEngine.Services.Implementations.Shared;

internal class MeshRendererFactory : IFactory<string, string, MeshRenderer>
{
    private readonly ILogger logger;
    private readonly IDrawingCall drawingCall;
    private readonly IFactory<string, ModelData> modelFactory;
    private readonly IFactory<string, Material> materialFactory;

    public MeshRendererFactory(ILogger logger, IDrawingCall drawingCall, IFactory<string, ModelData> modelFactory, IFactory<string, Material> materialFactory)
    {
        this.logger = logger;
        this.drawingCall = drawingCall;
        this.modelFactory = modelFactory;
        this.materialFactory = materialFactory;
    }

    public bool Create(string modelName, string materialName, out MeshRenderer mesh)
    {
        if (!modelFactory.Create(modelName, out ModelData model))
        {
            logger.LogInformation("Falling Back to Default Model");
            if (!modelFactory.Create("MissingModel.obj", out model))
            {
                logger.LogCritical("Error While Falling Back to Default Model");
                mesh = default!;
                return false;
            }
        }

        if (!materialFactory.Create(materialName, out Material material))
        {
            logger.LogError("Falling Back to Default Material");
            if (!materialFactory.Create("Default.mat", out material))
            {
                logger.LogCritical("Error While Falling Back to Default Material");
                mesh = default!;
                return false;
            }
        }

        mesh = new MeshRenderer(drawingCall, model, material);
        return true;
    }
}