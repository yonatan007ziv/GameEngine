using GameEngine.Core.API;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;
using System.Drawing;
using System.Numerics;

namespace GameEngine.Services.Interfaces;

public interface IGameEngine
{
	IGraphicsEngine GraphicsEngine { get; }
	ISoundEngine SoundEngine { get; }
	IInputEngine InputEngine { get; }
	IPhysicsEngine PhysicsEngine { get; }

	string Title { get; set; }

	// Used for ui interactions, (-1, -1) bottom left and (1, 1) top right
	Vector2 NormalizedMousePosition { get; }

	bool LogRenderingLogs { get; set; }
	bool LogInputs { get; set; }
	bool LogFps { get; set; }
	bool LogTps { get; set; }

	// Future implementation
	bool DrawColliderGizmos { get; set; }

	int TickRate { get; set; }
	int FpsCap { get; set; }

	float FpsDeltaTimeStopper { get; }
	float ElapsedSeconds { get; }

	bool MouseLocked { get; set; }
	TaskCompletionSource EngineLoadingTask { get; }

	void Run();

	void SetResourceFolder(string path);
	void SetBackgroundColor(Color color);

	public WorldObject? GetWorldObjectFromId(int id);
	public UIObject? GetUIObjectFromId(int id);

	void AddWorldObject(WorldObject worldObject);
	void RemoveWorldObject(WorldObject worldObject);
	void AddUIObject(UIObject uiObject);
	void RemoveUIObject(UIObject uiObject);
	void AddWorldCamera(WorldObject camera, CameraRenderingMask<string> cameraRenderingMask, ViewPort viewport);
	void RemoveWorldCamera(WorldObject camera);
	void AddUICamera(UIObject camera, CameraRenderingMask<string> cameraRenderingMask, ViewPort viewport);
	void RemoveUICamera(UIObject camera);
}