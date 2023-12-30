using GraphicsRenderer.Components.Shared.Data;

namespace GraphicsRenderer.Services.Interfaces.Utils;

internal interface IModelImporter
{
	ModelData ImportModel(string model);
}