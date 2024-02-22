﻿using GameEngine.Core.SharedServices.Interfaces;

namespace GameEngine.Core.SharedServices.Implementations;

public class ResourceDiscoverer : IResourceDiscoverer
{
	public Dictionary<string, string> ResourceNamePathDictionary { get; } = new Dictionary<string, string>();

	public ResourceDiscoverer()
	{
		DiscoverResources(new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), @"Resources\")), @"Resources\");
	}

	private void DiscoverResources(DirectoryInfo directory, string depth)
	{
		foreach (FileInfo file in directory.GetFiles())
			ResourceNamePathDictionary.Add(file.Name, Path.Combine(depth, file.Name));

		foreach (DirectoryInfo subDirectory in directory.GetDirectories())
			DiscoverResources(subDirectory, Path.Combine(depth, subDirectory.Name));
	}
}