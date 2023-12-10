namespace OpenGLRenderer.Services.Interfaces.Utils.Managers;

internal interface IResourceManager
{
	bool ResourceExists(string resource);
	string[] LoadResource(string resource);
}