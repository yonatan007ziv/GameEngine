using GraphicsRenderer.Components.Shared.Data;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;

namespace GraphicsRenderer.Services.Implementations.Utils.ModelImporter;

internal class ModelImporter : IModelImporter
{
    private readonly IResourceManager resourceManager;
    private readonly ObjModelImporter objModelImporter;
    private readonly FbxModelImporter fbxModelImporter;

    public ModelImporter(IResourceManager resourceManager, ObjModelImporter objModelImporter, FbxModelImporter fbxModelImporter)
    {
        this.resourceManager = resourceManager;
        this.objModelImporter = objModelImporter;
        this.fbxModelImporter = fbxModelImporter;
    }

    public Model3DData ImportModel(string model)
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