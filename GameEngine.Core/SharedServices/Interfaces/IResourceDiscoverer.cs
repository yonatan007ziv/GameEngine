namespace GameEngine.Core.SharedServices.Interfaces;

public interface IResourceDiscoverer
{
    void AddResourceFolder(string folderPath);
    string GetResourcePath(string resource);
    bool ResourceExists(string fontName);
}