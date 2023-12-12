using Microsoft.Extensions.Logging;
using OpenGLRenderer.Services.Interfaces.Utils;

namespace OpenGLRenderer.Services.Implementations.Utils;

internal class StringFileReader : IFileReader<string>
{
	private readonly ILogger logger;
	private readonly IPerformanceAnalyzer performanceAnalyzer;

	public StringFileReader(ILogger logger, IPerformanceAnalyzer performanceAnalyzer)
	{
		this.logger = logger;
		this.performanceAnalyzer = performanceAnalyzer;

		this.performanceAnalyzer.Logging = false;
	}

	public bool ReadFile(string filePath, out string[] lines)
	{
		try
		{
			performanceAnalyzer.StartSegment(0);

			lines = File.ReadLines(filePath).ToArray();

			performanceAnalyzer.StopSegment(0);
			performanceAnalyzer.Log();

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
			performanceAnalyzer.StartSegment(0);

			file = File.ReadAllText(filePath);

			performanceAnalyzer.StopSegment(0);
			performanceAnalyzer.Log();

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