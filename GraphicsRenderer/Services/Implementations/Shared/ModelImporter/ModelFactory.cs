using GraphicsRenderer.Components.Shared.Data;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;

namespace GraphicsRenderer.Services.Implementations.Shared.ModelImporter;

internal class ModelFactory : IFactory<string, ModelData>
{
	private readonly IResourceManager resourceManager;
	private readonly ObjModelImporter objModelImporter;
	private readonly FbxModelImporter fbxModelImporter;

	public ModelFactory(IResourceManager resourceManager, ObjModelImporter objModelImporter, FbxModelImporter fbxModelImporter)
	{
		this.resourceManager = resourceManager;
		this.objModelImporter = objModelImporter;
		this.fbxModelImporter = fbxModelImporter;
	}

	public ModelData Create(string model)
	{
		if (!resourceManager.ResourceExists(model))
			throw new Exception();

		string[] data = resourceManager.LoadResourceLines(model);

		string type = model.Split('.')[1];
		if (type == "obj")
			return objModelImporter.Import(data);
		else if (type == "fbx")
			return fbxModelImporter.Import(data);
		throw new Exception();
	}
}