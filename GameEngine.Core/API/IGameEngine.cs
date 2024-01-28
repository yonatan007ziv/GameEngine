namespace GameEngine.Core.API;

public interface IGameEngine
{
	public int TickRate { get; set; }
	public int FpsCap { get; set; }

	public float FpsDeltaTime { get; }
	public float ElapsedSeconds { get; }

	void Run();
}