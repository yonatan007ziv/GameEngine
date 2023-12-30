using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;

namespace GraphicsRenderer.Services.Implementations.Shared.Managers;

internal class ContentResourceManager : IResourceManager
{
	private readonly Dictionary<string, string> resourceNamePathDictionary = new Dictionary<string, string>();
	private readonly IFileReader<string> stringFileReader;

	public ContentResourceManager(IFileReader<string> stringFileReader)
	{
		this.stringFileReader = stringFileReader;

		DiscoverResources(new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), @"Resources\")), @"Resources\");
	}

	private void DiscoverResources(DirectoryInfo directory, string depth)
	{
		foreach (FileInfo file in directory.GetFiles())
			resourceNamePathDictionary.Add(file.Name, Path.Combine(depth, file.Name));

		foreach (DirectoryInfo subDirectory in directory.GetDirectories())
			DiscoverResources(subDirectory, Path.Combine(depth, subDirectory.Name));
	}

	public string[] LoadResourceLines(string resource)
	{
		if (!ResourceExists(resource))
			throw new Exception();

		if (stringFileReader.ReadFile(resourceNamePathDictionary[resource], out string[] lines))
			return lines;

		throw new Exception();
	}

	public string LoadResourceString(string resource)
	{
		if (!ResourceExists(resource))
			throw new Exception();

		if (stringFileReader.ReadFile(resourceNamePathDictionary[resource], out string file))
			return file;

		throw new Exception();
	}

	public bool ResourceExists(string resource)
		=> resourceNamePathDictionary.ContainsKey(resource);

}