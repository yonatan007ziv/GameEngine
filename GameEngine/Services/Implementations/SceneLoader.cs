using GameEngine.Components;
using GameEngine.Core.SharedServices.Interfaces.Utils.Managers;
using GameEngine.Services.Interfaces.Parsers;
using Microsoft.Extensions.Logging;

namespace GameEngine.Services.Implementations;

internal class SceneLoader
{
	private readonly ILogger logger;
	private readonly ISerializer serializer;
	private readonly IResourceManager resourceManager;

	public SceneLoader(ILogger logger, ISerializer serializer, IResourceManager resourceManager)
	{
		this.logger = logger;
		this.serializer = serializer;
		this.resourceManager = resourceManager;
	}

	public SceneData LoadScene(string sceneName)
	{
		if (!resourceManager.LoadResourceString(sceneName, out string scene))
		{
			logger.LogError("Scene {sceneName} not found", sceneName);
			if (!resourceManager.LoadResourceString("Empty.scene", out scene))
			{
				logger.LogCritical("Error falling back to empty scene", sceneName);
				return default!;
			}
		}
		return serializer.Deserialize<SceneData>(scene);
	}
}