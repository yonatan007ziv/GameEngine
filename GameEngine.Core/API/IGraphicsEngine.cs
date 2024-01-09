using GameEngine.Core.Components;

namespace GameEngine.Core.API;

public interface IGraphicsEngine
{
	List<GameObject> RenderingObjects { get; }
	Transform CameraTransform { get; }

	string Title { get; set; }
	void Start();

	void RenderFrame();
	void SyncState();

	public void LockMouse(bool lockMouse);
	public void TurnVSync(bool vsync);
}