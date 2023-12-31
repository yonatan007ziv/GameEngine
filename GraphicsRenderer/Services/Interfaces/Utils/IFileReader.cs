namespace GraphicsRenderer.Services.Interfaces.Utils;

internal interface IFileReader<T>
{
	bool ReadFile(string filePath, out T? result);
}