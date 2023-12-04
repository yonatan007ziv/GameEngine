using OpenGLRenderer.Models;

namespace OpenGLRenderer.Services.Interfaces.OpenGL;

internal interface IModelImporter
{
	ModelData ImportModel(string path);
}