using OpenGLRenderer.Models;
using OpenGLRenderer.Services.Interfaces.Utils;

namespace OpenGLRenderer.Services.Implementations.Utils;

internal class SceneFactory : IFactory<Scene>
{
	private readonly IFileReader<string> fileReader;

	public SceneFactory(IFileReader<string> fileReader)
	{
		this.fileReader = fileReader;
	}

	public Scene Create()
	{
		return new Scene(fileReader);
	}
}