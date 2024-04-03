using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Events;
using GameEngine.Core.Components.Objects;
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

	void Run();

	void SetLockedMouse(bool lockMouse);
	void SetBackgroundColor(Color color);

	void RenderFrame();
	void DeleteFinalizedBuffers();

	void AddWorldObject(WorldObject worldObjectData);
	void RemoveWorldObject(WorldObject worldObjectData);
	void AddUIObject(UIObject uiObjectData);
	void RemoveUIObject(UIObject uiObjectData);

	void AddWorldCamera(WorldObject worldCamera, CameraRenderingMask renderingMask, ViewPort viewPort);
	void RemoveWorldCamera(WorldObject worldCamera);
	void AddUICamera(UIObject uiCamera, CameraRenderingMask renderingMask, ViewPort viewPort);
	void RemoveUICamera(UIObject uiCamera);
}