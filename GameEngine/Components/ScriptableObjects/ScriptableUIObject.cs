using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Buttons;
using GameEngine.Core.Components.Objects;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;

namespace GameEngine.Components.ScriptableObjects;

public abstract class ScriptableUIObject : UIObject
{
	protected static bool MouseLocked { get => Services.Implementations.GameEngine.EngineContext.MouseLocked; set => Services.Implementations.GameEngine.EngineContext.MouseLocked = value; }

	public ObservableCollection<WorldObject> WorldObjects
	{
		get
		{
			if (Scene.LoadedScene is not null)
				return Scene.LoadedScene.WorldObjects;
			return Scene.WorldObjectsQueue;
		}
	}
	public ObservableCollection<(WorldCamera worldCamera, ViewPort viewPort)> WorldCameras
	{
		get
		{
			if (Scene.LoadedScene is not null)
				return Scene.LoadedScene.WorldCameras;
			return Scene.WorldCamerasQueue;
		}
	}
	public ObservableCollection<UIObject> UIObjects
	{
		get
		{
			if (Scene.LoadedScene is not null)
				return Scene.LoadedScene.UIObjects;
			return Scene.UIObjectsQueue;
		}
	}
	public ObservableCollection<(UICamera uiCamera, ViewPort viewPort)> UICameras
	{
		get
		{
			if (Scene.LoadedScene is not null)
				return Scene.LoadedScene.UICameras;
			return Scene.UICamerasQueue;
		}
	}

	public ScriptableUIObject() { }
	public ScriptableUIObject(UIObject parent)
		: base(parent) { }

	public static WorldObject? GetWorldObjectFromId(int id)
		=> Services.Implementations.GameEngine.EngineContext.GetWorldObjectFromId(id);
	public static UIObject? GetUIObjectFromId(int id)
		=> Services.Implementations.GameEngine.EngineContext.GetUIObjectFromId(id);

	public static Vector2 GetUIMousePosition()
		=> Services.Implementations.GameEngine.EngineContext.NormalizedMousePosition;
	public static Vector2 GetMousePosition()
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.GetMousePos();

	public static float GetAxis(string axis)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.GetAxis(axis);
	public static float GetAxisRaw(string axis)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.GetAxisRaw(axis);

	public static bool GetButtonPressed(string buttonName)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.GetButtonPressed(buttonName);
	public static bool GetButtonDown(string buttonName)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.GetButtonDown(buttonName);

	public static bool GetMouseButtonPressed(MouseButton mouseButton)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.GetMouseButtonPressed(mouseButton);
	public static bool GetMouseButtonDown(MouseButton mouseButton)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.GetMouseButtonDown(mouseButton);

	public static string CaptureKeyboardInput(string input)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.CaptureKeyboardInput(input);

	public static bool GetKeyboardButtonPressed(KeyboardButton keyboardButton)
	=> Services.Implementations.GameEngine.EngineContext.InputEngine.GetKeyboardButtonPressed(keyboardButton);
	public static bool GetKeyboardButtonDown(KeyboardButton keyboardButton)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.GetKeyboardButtonDown(keyboardButton);

	public static bool GetJoystickButtonPressed(GamepadButton joystickButton)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.GetGamepadButtonPressed(joystickButton);
	public static bool GetJoystickButtonDown(GamepadButton joystickButton)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.GetGamepadButtonDown(joystickButton);

	public static void SetBackgroundColor(Color color)
		=> Services.Implementations.GameEngine.EngineContext.SetWindowBackgroundColor(color);

	// The scriptable part of the UIObject
	public abstract void Update(float deltaTime);
}