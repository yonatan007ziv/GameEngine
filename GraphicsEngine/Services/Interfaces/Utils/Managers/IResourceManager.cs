namespace GraphicsEngine.Services.Interfaces.Utils.Managers;

public interface IResourceManager
{
	bool LoadResourceFileStream(string resource, out FileStream result);
	bool LoadResourceLines(string resource, out string[] result);
	bool LoadResourceString(string resource, out string result);
}