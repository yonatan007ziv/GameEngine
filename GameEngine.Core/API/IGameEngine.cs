namespace GameEngine.Core.API;

public interface IGameEngine
{
	public float ElapsedMs { get; }

	void Run();
}