using OpenGLRenderer.Models;
using OpenGLRenderer.Services.Interfaces.OpenGL;
using OpenGLRenderer.Services.Interfaces.Utils.Managers;

namespace OpenGLRenderer.Services.Implementations.OpenGL.ModelImporters;

internal class ModelImporter : IModelImporter
{
	private readonly ObjModelImporter objModelImporter;
	private readonly FbxModelImporter fbxModelImporter;
	private readonly IResourceManager resourceManager;

	public ModelImporter(ObjModelImporter objModelImporter, FbxModelImporter fbxModelImporter, IResourceManager resourceManager)
	{
		this.objModelImporter = objModelImporter;
		this.fbxModelImporter = fbxModelImporter;
		this.resourceManager = resourceManager;
	}

	public ModelData ImportModel(string model)
	{
		if (!resourceManager.ResourceExists(model))
			throw new Exception();

		string[] data = resourceManager.LoadResource(model);

		string type = model.Split('.')[1];
		if (type == "obj")
			return objModelImporter.Import(data);
		else if (type == "fbx")
			return fbxModelImporter.Import(data);
		throw new Exception();
	}
}