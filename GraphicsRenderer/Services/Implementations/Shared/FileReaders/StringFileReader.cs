using GraphicsRenderer.Services.Interfaces.Utils;
using Microsoft.Extensions.Logging;

namespace GraphicsRenderer.Services.Implementations.Shared.FileReaders;

internal class StringFileReader : IFileReader<string>
{
	private readonly ILogger logger;

	public StringFileReader(ILogger logger)
	{
		this.logger = logger;
	}

	public bool ReadFile(string filePath, out string? result)
	{
		try
		{
			result = File.ReadAllText(filePath);
			return true;
		}
		catch (FileNotFoundException)
		{
			logger.LogError("File at {filePath} not found", filePath);
		}
		catch (Exception ex)
		{
			logger.LogError("Error loading file at {filePath}\nError: {exceptionString}", filePath, ex.ToString());
		}

		result = null;
		return false;
	}
}