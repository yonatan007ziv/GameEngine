using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Events;
using System.Drawing;

namespace GameEngine.Core.API;

public interface IGraphicsEngine
{
	event Action Load;

	event Action<MouseEventData>? MouseEvent;
	event Action<KeyboardEventData>? KeyboardEvent;
	event Action<GamepadEventData>? GamepadEvent;

	string Title { get; set; }
	IntPtr WindowHandle { get; }
	bool LogRenderingMessages { get; set; }

	void Start();

	void SetBackgroundColor(Color color);

	void RenderFrame();
	void UpdateObject(ref GameObjectData gameObject);

	void AddCamera(ref GameComponentData gameObjectData, ViewPort viewPort);
	void RemoveCamera(ref GameComponentData gameObjectData);

	void AddGameObject(ref GameObjectData gameObject);
	void RemoveGameObject(ref GameObjectData gameObjectData);

	void LockMouse(bool lockMouse);
}