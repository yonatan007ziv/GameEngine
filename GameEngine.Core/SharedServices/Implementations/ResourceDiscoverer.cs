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

	public void InitResourceFolder(string folderPath)
	{
		// If resources folder doesn't exist
		if (!Directory.Exists(folderPath))
			Directory.CreateDirectory(folderPath);

		DiscoverResources(new DirectoryInfo(folderPath), @"Resources\");
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

	private void DiscoverResources(DirectoryInfo directory, string depth)
	{
		foreach (FileInfo file in directory.GetFiles())
			resourceNamePathDictionary.Add(file.Name, Path.Combine(depth, file.Name));

		foreach (DirectoryInfo subDirectory in directory.GetDirectories())
			DiscoverResources(subDirectory, Path.Combine(depth, subDirectory.Name));
	}
}