using GraphicsRenderer.Components.Shared.Data;

namespace GraphicsRenderer.Services.Interfaces.Utils;

internal interface IModelImporter
{
	Model3DData ImportModel(string model);
}