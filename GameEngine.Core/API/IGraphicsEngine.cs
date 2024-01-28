using GameEngine.Core.Components;

namespace GameEngine.Core.API;

public interface IGraphicsEngine
{
	string Title { get; set; }
	IntPtr WindowHandle { get; }

	void Start();

	void RenderFrame();

	void RegisterObject(ref GameObjectData gameObject);
	void UpdateObject(ref GameObjectData gameObject);

	void RegisterCameraGameObject(ref GameObjectData gameObjectData, Core.Components.ViewPort viewPort);
	void LockMouse(bool lockMouse);

	// maybe temp?
	void SetKeyboardCallback(Action<char> keyCallback);
}