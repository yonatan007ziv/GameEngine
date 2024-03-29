namespace GameEngine.Core.SharedServices.Interfaces;

public interface IFileReader<T>
{
	bool ReadFile(string filePath, out T result);
}