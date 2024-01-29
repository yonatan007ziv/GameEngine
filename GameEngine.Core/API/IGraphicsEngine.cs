using GameEngine.Core.Components;
using GameEngine.Core.Components.Input;
using System.Numerics;
using static GameEngine.Core.Components.Delegates;

namespace GameEngine.Core.API;

public interface IGraphicsEngine
{
	public event Action<Vector2>? MousePositionEvent;
	public event RefAction<MouseEvent>? MouseButtonEvent;
	public event RefAction<KeyboardEvent>? KeyboardButtonEvent;

	string Title { get; set; }
	IntPtr WindowHandle { get; }

	void Start();

	void RenderFrame();

	void RegisterObject(ref GameObjectData gameObject);
	void UpdateObject(ref GameObjectData gameObject);

	void RegisterCameraGameObject(ref GameObjectData gameObjectData, ViewPort viewPort);
	void LockMouse(bool lockMouse);
}