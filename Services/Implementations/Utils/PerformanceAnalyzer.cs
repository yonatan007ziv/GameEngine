using Microsoft.Extensions.Logging;
using OpenGLRenderer.Services.Interfaces.Utils;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace OpenGLRenderer.Services.Implementations.Utils;

internal class PerformanceAnalyzer : IPerformanceAnalyzer
{
	private readonly ILogger logger;
	private readonly List<Stopwatch> sw;

	public bool Logging { get; set; }

	public PerformanceAnalyzer(ILogger logger)
	{
		this.logger = logger;

		sw = new List<Stopwatch>();
	}

	public void Log([CallerMemberName] string callerName = "")
	{
		if (Logging)
			for (int i = 0; i < sw.Count; i++)
				logger.LogDebug("{caller} Segment {segment} Time: {timePassed}",
					callerName, i, sw[i].ElapsedMilliseconds);
	}

	public void StartSegment(int segment)
	{
		if (sw.Count == segment)
			sw.Add(new Stopwatch());
		sw[segment].Start();
	}

	public void RestartSegment(int segment)
	{
		if (sw.Count == segment)
			sw.Add(new Stopwatch());
		sw[segment].Restart();
	}

	public void StopSegment(int segment)
	{
		if (sw.Count == segment)
			sw.Add(new Stopwatch());
		sw[segment].Stop();
	}
}