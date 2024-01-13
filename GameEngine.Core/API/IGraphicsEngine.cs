using GameEngine.Core.Components.CommunicationComponentsData;

namespace GameEngine.Core.API;

public interface IGraphicsEngine
{
	string Title { get; set; }
	void Start();

	void RenderFrame();
	void Update();

	void SetCameraParent(ref GameObjectData gameObject);
	void UpdateGameObject(ref GameObjectData gameObject);
	void RegisterGameObject(ref GameObjectData gameObject);

	public void LockMouse(bool lockMouse);
	public void TurnVSync(bool vsync);
}