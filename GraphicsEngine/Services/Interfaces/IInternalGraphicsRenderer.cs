using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Events;
using GameEngine.Core.SharedServices.Interfaces;
using GraphicsEngine.Components.Interfaces;
using System.Drawing;
using System.Numerics;

namespace GraphicsEngine.Services.Interfaces;

internal interface IInternalGraphicsRenderer
{
	IFactory<string, string, IMeshRenderer> MeshFactory { get; }

	event Action LoadEvent;
	event Action ResizedEvent;

	event Action<MouseEventData>? MouseEvent;
	event Action<KeyboardEventData>? KeyboardEvent;
	event Action<GamepadEventData>? GamepadEvent;

	IntPtr WindowHandle { get; }
	Vector2 WindowSize { get; }
	string Title { get; set; }
	bool LogRenderingMessages { get; set; }

	void Start();
	void ProcessEvents();
	void SwapBuffers();
	void SetMouseLocked(bool locked);
	void SetBackgroundColor(Color color);
	void SetDepthTest(bool enable);
	void SetViewport(ViewPort viewport);
}