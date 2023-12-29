using System.Runtime.CompilerServices;

namespace GraphicsRenderer.Services.Interfaces.Utils;

internal interface IPerformanceAnalyzer
{
	public bool Logging { get; set; }

	void Log([CallerMemberName] string callerName = "");
	void StartSegment(int segment);
	void RestartSegment(int segment);
	void StopSegment(int segment);
}