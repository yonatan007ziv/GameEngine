using GameEngine.Core.SharedServices.Interfaces;
using Microsoft.Extensions.Logging;

namespace GameEngine.Core.SharedServices.Implementations;

public class ResourceDiscoverer : IResourceDiscoverer
{
	private readonly ILogger logger;

	private Dictionary<string, string> resourceNamePathDictionary { get; } = new Dictionary<string, string>();
	private bool initializedFolder = false;

	public ResourceDiscoverer(ILogger logger)
	{
		this.logger = logger;
	}

	public void AddResourceFolder(string folderPath)
	{
		// If resources folder doesn't exist
		if (!Directory.Exists(folderPath))
		{
			logger.LogCritical("Resource path {folderPath} does not exist", folderPath);
			return;
		}

		DiscoverResources(new DirectoryInfo(folderPath));
		initializedFolder = true;
	}

	public string GetResourcePath(string resource)
	{
		if (!initializedFolder)
		{
			logger.LogCritical("Resource folder not initialized");
			Environment.Exit(0);
		}

		return resourceNamePathDictionary[resource];
	}

	public bool ResourceExists(string resource)
	{
		if (!initializedFolder)
		{
			logger.LogCritical("Resource folder not initialized");
			Environment.Exit(0);
		}

		return resourceNamePathDictionary.ContainsKey(resource);
	}

	private void DiscoverResources(DirectoryInfo directory)
	{
		foreach (FileInfo file in directory.GetFiles())
			if (resourceNamePathDictionary.ContainsKey(file.Name))
				logger.LogError("Resource with name {name} already exists", file.Name);
			else
				resourceNamePathDictionary.Add(file.Name, file.FullName);

		foreach (DirectoryInfo subDirectory in directory.GetDirectories())
			DiscoverResources(subDirectory);
	}
}