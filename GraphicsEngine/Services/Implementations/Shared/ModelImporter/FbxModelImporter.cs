using GraphicsEngine.Components.Shared.Data;
using Microsoft.Extensions.Logging;

namespace GraphicsEngine.Services.Implementations.Shared.ModelImporter;

internal class FbxModelImporter
{
	private readonly ILogger logger;

	public FbxModelImporter(ILogger logger)
	{
		this.logger = logger;
	}

	// Future Feature (maybe)
	public ModelData Import(string[] data)
	{
		logger.LogError("GraphicsEngine. Fbx model importing not supported yet");
		throw new NotImplementedException();
	}
}