using GameEngine.Core.SharedServices.Interfaces;
using GameEngine.Core.SharedServices.Interfaces.Utils.Managers;

namespace GameEngine.Core.SharedServices.Implementations.Managers;

public class ContentResourceManager : IResourceManager
{
    private readonly IResourceDiscoverer resourceDiscoverer;
    private readonly IFileReader<string> stringFileReader;
    private readonly IFileReader<FileStream> fileStreamFileReader;

    public ContentResourceManager(IResourceDiscoverer resourceDiscoverer, IFileReader<string> stringFileReader, IFileReader<FileStream> fileStreamFileReader)
    {
        this.resourceDiscoverer = resourceDiscoverer;
        this.stringFileReader = stringFileReader;
        this.fileStreamFileReader = fileStreamFileReader;
    }

    public bool LoadResourceFileStream(string resource, out FileStream result)
    {
        result = default!;

        if (!resourceDiscoverer.ResourceExists(resource))
            return false;

        if (fileStreamFileReader.ReadFile(resourceDiscoverer.GetResourcePath(resource), out FileStream stream))
        {
            result = stream;
            return true;
        }

        return false;
    }
    public bool LoadResourceString(string resource, out string result)
    {
        result = default!;

        if (!resourceDiscoverer.ResourceExists(resource))
            return false;

        if (stringFileReader.ReadFile(resourceDiscoverer.GetResourcePath(resource), out string file))
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
            for (int i = 0; i < result.Length; i++)
                result[i] = result[i].Trim();
            return true;
        }

        result = default!;
        return false;
    }
}