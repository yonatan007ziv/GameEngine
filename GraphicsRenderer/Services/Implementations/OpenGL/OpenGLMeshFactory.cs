using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Components.OpenGL;
using GraphicsRenderer.Components.Shared.Data;
using GraphicsRenderer.Services.Interfaces.Utils;

namespace GraphicsRenderer.Services.Implementations.OpenGL;

internal class OpenGLMeshFactory : IFactory<string, IMesh>
{
	private readonly IFactory<string, ModelData> modelImporter;

	public OpenGLMeshFactory(IFactory<string, ModelData> modelImporter)
	{
		this.modelImporter = modelImporter;
	}

	public bool Create(string modelName, out IMesh mesh)
	{
		if (!modelImporter.Create(modelName, out ModelData model))
		{
			mesh = default!;
			return false;
		}

		mesh = new OpenGLMesh(model);
		return true;
	}
}