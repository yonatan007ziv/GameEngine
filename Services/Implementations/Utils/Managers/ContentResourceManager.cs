using OpenGLRenderer.Services.Interfaces.Utils;
using OpenGLRenderer.Services.Interfaces.Utils.Managers;

namespace OpenGLRenderer.Services.Implementations.Utils.Managers;

internal class ContentResourceManager : IResourceManager
{
	private const string resourcePath = @"Resources\";

	private readonly Dictionary<string, string> cachedResources = new Dictionary<string, string>();
	private readonly IFileReader<string> stringFileReader;

	public ContentResourceManager(IFileReader<string> stringFileReader)
	{
		this.stringFileReader = stringFileReader;

		CacheResources(new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), @"Resources\")), @"Resources\");
	}

	private void CacheResources(DirectoryInfo directory, string depth)
	{
		foreach (FileInfo file in directory.GetFiles())
			cachedResources.Add(file.Name, Path.Combine(depth, file.Name));

		foreach (DirectoryInfo subDirectory in directory.GetDirectories())
			CacheResources(subDirectory, Path.Combine(depth, subDirectory.Name));
	}

	public string[] LoadResourceLines(string resource)
	{
		if (!ResourceExists(resource))
			throw new Exception();

		if (stringFileReader.ReadFile(cachedResources[resource], out string[] lines))
			return lines;

		throw new Exception();
	}

	public string LoadResourceString(string resource)
	{
		if (!ResourceExists(resource))
			throw new Exception();

		if (stringFileReader.ReadFile(cachedResources[resource], out string file))
			return file;

		throw new Exception();
	}

	public bool ResourceExists(string resource)
		=> cachedResources.ContainsKey(resource);

}