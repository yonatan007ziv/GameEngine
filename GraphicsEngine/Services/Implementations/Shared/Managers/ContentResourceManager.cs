using GraphicsEngine.Services.Interfaces.Utils;
using GraphicsEngine.Services.Interfaces.Utils.Managers;

namespace GraphicsEngine.Services.Implementations.Shared.Managers;

public class ContentResourceManager : IResourceManager
{
	private readonly Dictionary<string, string> resourceNamePathDictionary = new Dictionary<string, string>();
	private readonly IFileReader<string> stringFileReader;
	private readonly IFileReader<FileStream> fileStreamFileReader;

	public ContentResourceManager(IFileReader<string> stringFileReader, IFileReader<FileStream> fileStreamFileReader)
	{
		this.stringFileReader = stringFileReader;
		this.fileStreamFileReader = fileStreamFileReader;

		DiscoverResources(new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), @"Resources\")), @"Resources\");
	}

	private void DiscoverResources(DirectoryInfo directory, string depth)
	{
		foreach (FileInfo file in directory.GetFiles())
			resourceNamePathDictionary.Add(file.Name, Path.Combine(depth, file.Name));

		foreach (DirectoryInfo subDirectory in directory.GetDirectories())
			DiscoverResources(subDirectory, Path.Combine(depth, subDirectory.Name));
	}

	public bool LoadResourceFileStream(string resource, out FileStream result)
	{
		result = default!;

		if (!ResourceExists(resource))
			return false;

		if (fileStreamFileReader.ReadFile(resourceNamePathDictionary[resource], out FileStream stream))
		{
			result = stream;
			return true;
		}

		return false;
	}
	public bool LoadResourceString(string resource, out string result)
	{
		result = default!;

		if (!ResourceExists(resource))
			return false;

		if (stringFileReader.ReadFile(resourceNamePathDictionary[resource], out string file))
		{
			result = file;
			return true;
		}

		return false;
	}
	public bool LoadResourceLines(string resource, out string[] result)
	{
		if (LoadResourceString(resource, out string fileStr))
		{
			result = fileStr.Split('\n');
			return true;
		}

		result = default!;
		return false;
	}

	private bool ResourceExists(string resource)
		=> resourceNamePathDictionary.ContainsKey(resource);

}