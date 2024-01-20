using GameEngine.Core.Components;

namespace GameEngine.Core.API;

public interface IGraphicsEngine
{
	string Title { get; set; }
	IntPtr WindowHandle { get; }

	void Start();

	void RenderFrame();

	void UpdateGameObject(ref GameObjectData gameObject);

	void SetCamera(ref GameObjectData gameObjectData);
	void LockMouse(bool lockMouse);

	// maybe temp?
	void SetKeyboardCallback(Action<char> keyCallback);
}