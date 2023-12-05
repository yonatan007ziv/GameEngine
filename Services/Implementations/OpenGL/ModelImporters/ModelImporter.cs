using OpenGLRenderer.Models;
using OpenGLRenderer.Services.Interfaces.OpenGL;

namespace OpenGLRenderer.Services.Implementations.OpenGL.ModelImporters;

internal class ModelImporter : IModelImporter
{
	private readonly ObjModelImporter objModelImporter;

	public ModelImporter(ObjModelImporter objModelImporter)
	{
		this.objModelImporter = objModelImporter;
	}

	public ModelData ImportModel(string path)
	{
		string extension = Path.GetExtension(path);
		if (extension == ".obj")
			return objModelImporter.ImportObj(path);
		throw new Exception();
	}
}