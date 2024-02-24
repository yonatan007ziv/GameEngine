using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Events;
using System.Drawing;
using System.Numerics;

namespace GameEngine.Core.API;

public interface IGraphicsEngine
{
	event Action Load;

	event Action<MouseEventData>? MouseEvent;
	event Action<KeyboardEventData>? KeyboardEvent;
	event Action<GamepadEventData>? GamepadEvent;

	string Title { get; set; }
	IntPtr WindowHandle { get; }
	Vector2 WindowSize { get; }
	bool LogRenderingMessages { get; set; }

	void Start();

	void SetBackgroundColor(Color color);

	void RenderFrame();
	void UpdateWorldObject(ref WorldObjectData worldObjectData);
	void UpdateUIObject(ref UIObjectData uiObjectData);

	void AddWorldCamera(ref GameComponentData worldCameraData, ViewPort viewPort);
	void AddUICamera(ref GameComponentData uiCameraData, ViewPort viewPort);
	void RemoveWorldCamera(ref GameComponentData worldCameraData);
	void RemoveUICamera(ref GameComponentData uiCameraData);

	void AddWorldObject(ref WorldObjectData worldObjectData);
	void AddUIObject(ref UIObjectData uiObjectData);
	void RemoveWorldObject(ref WorldObjectData worldObjectData);
	void RemoveUIObject(ref UIObjectData uiObjectData);

	void LockMouse(bool lockMouse);
}