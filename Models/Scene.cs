using OpenGLRenderer.Services.Interfaces.Utils;

namespace OpenGLRenderer.Models;

internal class Scene
{
	private readonly IFileReader<string> fileReader;

	public Scene(IFileReader<string> fileReader)
	{
		this.fileReader = fileReader;
	}

	public void Load(string sceneName)
	{
		if (!fileReader.ReadFile(sceneName, out string[] lines)) { }

	}
}