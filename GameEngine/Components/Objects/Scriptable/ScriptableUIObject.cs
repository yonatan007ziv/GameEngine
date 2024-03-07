using GameEngine.Core.Components.Input.Buttons;
using System.Drawing;
using System.Numerics;

namespace GameEngine.Components.Objects.Scriptable;

public abstract class ScriptableUIObject : UIObject
{
	protected static bool MouseLocked { get => Services.Implementations.GameEngine.EngineContext.MouseLocked; set => Services.Implementations.GameEngine.EngineContext.MouseLocked = value; }

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

	public static string GetRecentKeyboardInput()
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.GetRecentKeyboardInput();

	public static bool GetKeyboardButtonPressed(KeyboardButton keyboardButton)
	=> Services.Implementations.GameEngine.EngineContext.InputEngine.GetKeyboardButtonPressed(keyboardButton);
	public static bool GetKeyboardButtonDown(KeyboardButton keyboardButton)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.GetKeyboardButtonDown(keyboardButton);

	public static bool GetJoystickButtonPressed(GamepadButton joystickButton)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.GetGamepadButtonPressed(joystickButton);
	public static bool GetJoystickButtonDown(GamepadButton joystickButton)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.GetGamepadButtonDown(joystickButton);

	public static void SetBackgroundColor(Color color)
		=> Services.Implementations.GameEngine.EngineContext.SetBackgroundColor(color);

	public abstract void Update(float deltaTime);
}