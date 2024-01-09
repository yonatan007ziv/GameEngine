namespace GraphicsEngine.Services.Interfaces.Utils;

public interface IFileReader<T>
{
	bool ReadFile(string filePath, out T result);
}