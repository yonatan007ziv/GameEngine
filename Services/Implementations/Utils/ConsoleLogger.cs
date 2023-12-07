using Microsoft.Extensions.Logging;

namespace OpenGLRenderer.Services.Implementations.Utils;

internal class ConsoleLogger : ILogger
{
	public IDisposable? BeginScope<TState>(TState state) where TState : notnull
	{
		throw new NotImplementedException();
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		throw new NotImplementedException();
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
	{
		string message = formatter(state, exception);

		ConsoleColor clr;

		switch (logLevel)
		{
			default:
				clr = ConsoleColor.White;
				break;
			case LogLevel.Trace:
				clr = ConsoleColor.DarkGreen;
				break;
			case LogLevel.Debug:
				clr = ConsoleColor.Cyan;
				break;
			case LogLevel.Information:
				clr = ConsoleColor.Green;
				break;
			case LogLevel.Warning:
				clr = ConsoleColor.Yellow;
				break;
			case LogLevel.Error:
				clr = ConsoleColor.Red;
				break;
			case LogLevel.Critical:
				clr = ConsoleColor.DarkRed;
				break;
			case LogLevel.None:
				clr = ConsoleColor.White;
				break;
		}
		Console.ForegroundColor = clr;
		Console.WriteLine("{0}: {1}", logLevel.ToString(), message);
		Console.ForegroundColor = ConsoleColor.White;
	}
}