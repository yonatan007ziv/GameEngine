using GameEngine.Core.SharedServices.Interfaces;
using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Components.OpenGL;
using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Components.Shared.Data;
using Microsoft.Extensions.Logging;

namespace GraphicsRenderer.Services.Implementations.OpenGL;

internal class OpenGLMeshRendererFactory : IFactory<string, string, IMeshRenderer>
{
	private readonly ILogger logger;
	private readonly IFactory<string, ModelData> modelImporter;
	private readonly IFactory<string, Material> materialFactory;

	public OpenGLMeshRendererFactory(ILogger logger, IFactory<string, ModelData> modelImporter, IFactory<string, Material> materialFactory)
	{
		this.logger = logger;
		this.modelImporter = modelImporter;
		this.materialFactory = materialFactory;
	}

	public bool Create(string modelName, string shaderName, out IMeshRenderer mesh)
	{
		if (!modelImporter.Create(modelName, out ModelData model))
		{
			logger.LogError("Falling Back to Default Model");
			if (!modelImporter.Create("MissingModel.obj", out model))
			{
				logger.LogCritical("Error While Falling Back to Default Model");
				mesh = default!;
				return false;
			}
		}

		if (!materialFactory.Create(shaderName, out Material material))
		{
			logger.LogError("Falling Back to Default Material");
			if (!materialFactory.Create("Default", out material))
			{
				logger.LogCritical("Error While Falling Back to Default Material");
				mesh = default!;
				return false;
			}
		}

		mesh = new OpenGLMeshRenderer(model, material);
		return true;
	}
}