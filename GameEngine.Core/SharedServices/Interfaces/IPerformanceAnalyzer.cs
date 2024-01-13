using System.Runtime.CompilerServices;

namespace GameEngine.Core.SharedServices.Interfaces;

public interface IPerformanceAnalyzer
{
	public bool Logging { get; set; }

	void Log([CallerMemberName] string callerName = "");
	void StartSegment(int segment);
	void RestartSegment(int segment);
	void StopSegment(int segment);
}