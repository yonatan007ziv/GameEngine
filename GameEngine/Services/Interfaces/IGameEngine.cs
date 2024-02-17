using GameEngine.Components.Objects;
using GameEngine.Core.API;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Buttons;
using System.Drawing;

namespace GameEngine.Services.Interfaces;

public interface IGameEngine
{
	IInputEngine InputEngine { get; }

	string Title { get; set; }

	bool LogRenderingLogs { get; set; }
	bool LogInputs { get; set; }
	bool LogFps { get; set; }
	bool LogTps { get; set; }

	int TickRate { get; set; }
	int FpsCap { get; set; }

	float FpsDeltaTimeStopper { get; }
	float ElapsedSeconds { get; }

	bool MouseLocked { get; set; }
	TaskCompletionSource EngineLoadingTask { get; }

	void Run();
	void SetBackgroundColor(Color color);

	void AddWorldObject(WorldObject worldObject);
	void RemoveWorldObject(WorldObject worldObject);
	void AddUIObject(UIObject uiObject);
	void RemoveUIObject(UIObject uiObject);
	void AddWorldCamera(WorldComponent cameraObject, ViewPort viewport);
	void RemoveWorldCamera(WorldComponent cameraObject);
	void AddUICamera(UIComponent cameraObject, ViewPort viewport);
	void RemoveUICamera(UIComponent cameraObject);

	#region Input polling
	bool IsMouseButtonPressed(MouseButton mouseButton);
	bool IsMouseButtonDown(MouseButton mouseButton);

	bool IsKeyboardButtonPressed(KeyboardButton keyboardButton);
	bool IsKeyboardButtonDown(KeyboardButton keyboardButton);
	#endregion
}