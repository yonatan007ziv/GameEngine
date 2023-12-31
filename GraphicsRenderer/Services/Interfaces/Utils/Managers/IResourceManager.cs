namespace GraphicsRenderer.Services.Interfaces.Utils.Managers;

internal interface IResourceManager
{
	bool ResourceExists(string resource);
	FileStream LoadResourceFileStream(string resource);
	string[] LoadResourceLines(string resource);
	string LoadResourceString(string resource);
}