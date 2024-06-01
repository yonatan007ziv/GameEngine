using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Events;
using GameEngine.Core.Components.Objects;
using System.Drawing;
using System.Numerics;

namespace GameEngine.Core.API;

public interface IGraphicsEngine
{
	event Action Load; // Fires up when the engine first loads up

	event Action<MouseEventData>? MouseEvent; // Called when mouse event has occurred
	event Action<KeyboardEventData>? KeyboardEvent; // Called when keyboard event has occurred
	event Action<GamepadEventData>? GamepadEvent; // Called when gamepad event has occurred
	Action<Vector2> ScreenSizeChanged { get; set; } // Called when window resize event has occurred

	string Title { get; set; } // The title of the window
	IntPtr WindowHandle { get; } // The handle of the window
	Vector2 WindowSize { get; } // The size of the window
	bool LogRenderingMessages { get; set; } // Whether to log rendering messages or not

	void Run(); // Runs the graphics engine

	void SetLockedMouse(bool lockMouse); // Locks/Frees the mouse from the window
	void SetBackgroundColor(Color color); // Sets the window's background color

	void RenderFrame(); // Render passes a frame
	void DeleteFinalizedBuffers(); // Deletes all unused unmanaged buffers

	#region Object management
	void AddWorldObject(WorldObject worldObjectData);
	void RemoveWorldObject(WorldObject worldObjectData);
	void AddUIObject(UIObject uiObjectData);
	void RemoveUIObject(UIObject uiObjectData);

	void AddWorldCamera(WorldObject worldCamera, CameraRenderingMask renderingMask, ViewPort viewPort);
	void RemoveWorldCamera(WorldObject worldCamera);
	void AddUICamera(UIObject uiCamera, CameraRenderingMask renderingMask, ViewPort viewPort);
	void RemoveUICamera(UIObject uiCamera);
	#endregion
}