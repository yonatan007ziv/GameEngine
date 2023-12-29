using GraphicsRenderer.Services.Interfaces.Utils;
using Microsoft.Extensions.Logging;

namespace GraphicsRenderer.Services.Implementations.Utils;

internal class StringFileReader : IFileReader<string>
{
	private readonly ILogger logger;

	public StringFileReader(ILogger logger)
	{
		this.logger = logger;
	}

	public bool ReadFile(string filePath, out string[] lines)
	{
		try
		{
			lines = File.ReadLines(filePath).ToArray();

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

		lines = Array.Empty<string>();
		return false;
	}

	public bool ReadFile(string filePath, out string file)
	{
		try
		{
			file = File.ReadAllText(filePath);
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

		file = "";
		return false;
	}
}