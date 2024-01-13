using GameEngine.Core.SharedServices.Interfaces;
using Microsoft.Extensions.Logging;

namespace GameEngine.Core.SharedServices.Implementations.FileReaders;

public class FileStreamFileReader : IFileReader<FileStream>
{
	private readonly ILogger logger;

	public FileStreamFileReader(ILogger logger)
	{
		this.logger = logger;
	}

	public bool ReadFile(string filePath, out FileStream result)
	{
		try
		{
			if (File.Exists(filePath))
			{
				result = File.OpenRead(filePath);
				return true;
			}
		}
		catch (FileNotFoundException)

		{
			logger.LogError("File at {filePath} not found", filePath);
		}
		catch (Exception ex)
		{
			logger.LogError("Error loading file at {filePath}\nError: {exceptionString}", filePath, ex.ToString());
		}
		result = null!;
		return false;
	}
}