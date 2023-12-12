namespace OpenGLRenderer.Services.Interfaces.Utils;

internal interface IFileReader<T>
{
	bool ReadFile(string filePath, out T[] lines);
	bool ReadFile(string filePath, out T file);
}