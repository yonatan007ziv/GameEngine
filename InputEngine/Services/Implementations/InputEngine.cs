using GameEngine.Core.API;
using GameEngine.Core.Components.Input.Buttons;
using GameEngine.Core.Components.Input.Events;
using InputEngine.Components;
using Microsoft.Extensions.Logging;
using System.Numerics;

namespace InputEngine.Services.Implementations;

internal class InputEngine : IInputEngine
{
	private readonly ILogger logger;

	public bool LogInputs { get; set; }

	private readonly Dictionary<string, ButtonInputData> registeredButtons = new Dictionary<string, ButtonInputData>();
	private readonly Dictionary<string, AxisInputData> registeredAxes = new Dictionary<string, AxisInputData>();

	#region Input containers
	private readonly List<MouseButton> mouseButtons = new List<MouseButton>();
	private readonly List<KeyboardButton> keyboardButtons = new List<KeyboardButton>();
	private readonly List<GamepadButton> gamepadButtons = new List<GamepadButton>();

	private readonly List<MouseButton> mouseButtonsDownMask = new List<MouseButton>();
	private readonly List<KeyboardButton> keyboardButtonsDownMask = new List<KeyboardButton>();
	private readonly List<GamepadButton> gamepadButtonsDownMask = new List<GamepadButton>();

	private readonly Dictionary<MouseAxis, float> mouseAxes = new Dictionary<MouseAxis, float>();
	private readonly Dictionary<KeyboardButton, float> keyboardAxes = new Dictionary<KeyboardButton, float>();
	private readonly Dictionary<GamepadAxis, float> gamepadAxes = new Dictionary<GamepadAxis, float>();
	#endregion

	private Vector2 mousePosition, lastMousePosition;

	public InputEngine(ILogger logger)
	{
		this.logger = logger;
	}

	public void InputTickPass()
	{
		mouseButtonsDownMask.Clear();
		keyboardButtonsDownMask.Clear();
		gamepadButtonsDownMask.Clear();

		// Temp solution to clear mouse input each tick
		mouseAxes[MouseAxis.MouseHorizontal] = 0;
		mouseAxes[MouseAxis.MouseVertical] = 0;

		lastMousePosition = mousePosition;
	}

	public Vector2 GetMousePos()
		=> mousePosition;

	#region Get button pressed/down
	public bool GetButtonPressed(string buttonName)
	{
		if (!registeredButtons.ContainsKey(buttonName))
		{
			logger.LogError("InputEngine. No button registered for: \"{buttonName}\"", buttonName);
			return false;
		}

		ButtonInputData buttonInputData = registeredButtons[buttonName];

		if (buttonInputData.IsMouseRegistered && GetMouseButtonPressed(buttonInputData.MouseButton))
			return true;

		if (buttonInputData.IsKeyboardRegistered && GetKeyboardButtonPressed(buttonInputData.KeyboardButton))
			return true;

		if (buttonInputData.IsGamepadRegistered && GetGamepadButtonPressed(buttonInputData.GamepadButton))
			return true;

		return false;
	}
	public bool GetButtonDown(string buttonName)
	{
		if (!registeredButtons.ContainsKey(buttonName))
		{
			logger.LogError("InputEngine. No button registered for: \"{buttonName}\"", buttonName);
			return false;
		}

		ButtonInputData inputButtonData = registeredButtons[buttonName];

		if (inputButtonData.IsMouseRegistered && GetMouseButtonDown(inputButtonData.MouseButton))
			return true;

		if (inputButtonData.IsKeyboardRegistered && GetKeyboardButtonDown(inputButtonData.KeyboardButton))
			return true;

		if (inputButtonData.IsGamepadRegistered && GetGamepadButtonDown(inputButtonData.GamepadButton))
			return true;

		return false;
	}

	public bool GetMouseButtonPressed(MouseButton mouseButton)
		=> mouseButtons.Contains(mouseButton);
	public bool GetMouseButtonDown(MouseButton mouseButton)
		=> mouseButtonsDownMask.Contains(mouseButton) && mouseButtons.Contains(mouseButton);

	public bool GetKeyboardButtonPressed(KeyboardButton keyboardButton)
		=> keyboardButtons.Contains(keyboardButton);
	public bool GetKeyboardButtonDown(KeyboardButton keyboardButton)
		=> keyboardButtonsDownMask.Contains(keyboardButton) && keyboardButtons.Contains(keyboardButton);

	public bool GetGamepadButtonPressed(GamepadButton gamepadButton)
				=> gamepadButtons.Contains(gamepadButton);
	public bool GetGamepadButtonDown(GamepadButton gamepadButton)
				=> gamepadButtonsDownMask.Contains(gamepadButton) && gamepadButtons.Contains(gamepadButton);
	#endregion

	#region Get axis
	public float GetAxis(string axisName)
	{
		if (!registeredAxes.ContainsKey(axisName))
		{
			logger.LogError("InputEngine. No input mapped for axis: \"{axisName}\"", axisName);
			return 0;
		}

		AxisInputData axisInputData = registeredAxes[axisName];
		float axisSum = 0;

		if (axisInputData.IsMouseRegistered)
			axisSum += GetMouseAxis(axisInputData.MouseAxis) * axisInputData.MouseAxisMultiplier + axisInputData.MouseAxisOffset;

		if (axisInputData.IsKeyboardRegistered)
			axisSum += (GetKeyboardAxis(axisInputData.KeyboardAxisPositive) - GetKeyboardAxis(axisInputData.KeyboardAxisNegative)) * axisInputData.KeyboardAxisMultiplier + axisInputData.KeyboardAxisOffset;

		if (axisInputData.IsGamepadRegistered)
			axisSum += GetGamepadAxis(axisInputData.GamepadAxis) * axisInputData.GamepadAxisMultiplier + axisInputData.GamepadAxisOffset;

		return axisSum;
	}
	public float GetAxisRaw(string axis)
	{
		float axisValue = GetAxis(axis);
		if (axisValue > 0)
			return 1;
		else if (axisValue < 0)
			return -1;
		return 0;
	}
	public float GetMouseAxis(MouseAxis axis)
		=> mouseAxes.ContainsKey(axis) ? mouseAxes[axis] : 0f;
	public float GetKeyboardAxis(KeyboardButton axis)
		=> keyboardAxes.ContainsKey(axis) ? keyboardAxes[axis] : 0f;
	public float GetGamepadAxis(GamepadAxis axis)
		=> gamepadAxes.ContainsKey(axis) ? gamepadAxes[axis] : 0f;
	#endregion

	#region Input events
	public void OnMouseEvent(MouseEventData mouseEvent)
	{
		if (mouseEvent.MouseEventType == MouseEventType.MouseMove)
		{
			if (LogInputs)
				logger.LogInformation("Mouse: Moved");

			mouseAxes[MouseAxis.MouseHorizontal] = mouseEvent.MousePosition.X - lastMousePosition.X;
			mouseAxes[MouseAxis.MouseVertical] = mouseEvent.MousePosition.Y - lastMousePosition.Y;
			mousePosition = mouseEvent.MousePosition;
		}
		else if (mouseEvent.MouseEventType == MouseEventType.MouseButton)
		{
			if (LogInputs)
				logger.LogInformation("Mouse: {button} {pressed?}", mouseEvent.MouseButton, mouseEvent.Pressed ? "Pressed" : "Released");

			if (mouseEvent.Pressed)
			{
				if (!mouseButtons.Contains(mouseEvent.MouseButton))
				{
					mouseButtons.Add(mouseEvent.MouseButton);
					mouseButtonsDownMask.Add(mouseEvent.MouseButton);
				}
			}
			else
			{
				if (mouseButtons.Contains(mouseEvent.MouseButton))
				{
					mouseButtons.Remove(mouseEvent.MouseButton);
					mouseButtonsDownMask.Remove(mouseEvent.MouseButton);
				}
			}
		}
	}
	public void OnKeyboardEvent(KeyboardEventData keyboardEvent)
	{
		if (LogInputs)
			logger.LogInformation("Keyboard: {button} {pressed?}", keyboardEvent.KeyboardButton, keyboardEvent.Pressed ? "Pressed" : "Released");

		if (keyboardEvent.Pressed)
		{
			if (!keyboardButtons.Contains(keyboardEvent.KeyboardButton))
			{
				keyboardButtons.Add(keyboardEvent.KeyboardButton);
				keyboardButtonsDownMask.Add(keyboardEvent.KeyboardButton);

				keyboardAxes[keyboardEvent.KeyboardButton] = 1;
			}
		}
		else
		{
			if (keyboardButtons.Contains(keyboardEvent.KeyboardButton))
			{
				keyboardButtons.Remove(keyboardEvent.KeyboardButton);
				keyboardButtonsDownMask.Remove(keyboardEvent.KeyboardButton);

				keyboardAxes[keyboardEvent.KeyboardButton] = 0;
			}
		}
	}
	public void OnGamepadEvent(GamepadEventData gamepadEvent)
	{
		if (gamepadEvent.GamepadEventType == GamepadEventType.GamepadButton)
		{
			if (LogInputs)
				logger.LogInformation("Gamepad: {button} {pressed?}", gamepadEvent.GamepadButton, gamepadEvent.Pressed ? "Pressed" : "Released");

			if (gamepadEvent.Pressed)
			{
				if (!gamepadButtons.Contains(gamepadEvent.GamepadButton))
				{
					gamepadButtons.Add(gamepadEvent.GamepadButton);
					gamepadButtonsDownMask.Add(gamepadEvent.GamepadButton);
				}
			}
			else
			{
				if (gamepadButtons.Contains(gamepadEvent.GamepadButton))
				{
					gamepadButtons.Remove(gamepadEvent.GamepadButton);
					gamepadButtonsDownMask.Remove(gamepadEvent.GamepadButton);
				}
			}
		}
		else if (gamepadEvent.GamepadEventType == GamepadEventType.GamepadAnalog)
		{
			if (LogInputs)
				logger.LogInformation("Gamepad: {button} Moved", gamepadEvent.GamepadAxis);

			if (gamepadEvent.GamepadAxis == GamepadAxis.LeftAnalogHorizontal)
			{ // Updating left analog
				gamepadAxes[GamepadAxis.LeftAnalogHorizontal] = gamepadEvent.AnalogPosition.X;
				gamepadAxes[GamepadAxis.LeftAnalogVertical] = gamepadEvent.AnalogPosition.Y;
			}
			else if (gamepadEvent.GamepadAxis == GamepadAxis.RightAnalogHorizontal)
			{ // Updating right analog
				gamepadAxes[GamepadAxis.RightAnalogHorizontal] = gamepadEvent.AnalogPosition.X;
				gamepadAxes[GamepadAxis.RightAnalogVertical] = gamepadEvent.AnalogPosition.Y;
			}
		}
		else if (gamepadEvent.GamepadEventType == GamepadEventType.GamepadTrigger)
		{
			if (LogInputs)
				logger.LogInformation("Gamepad: {button} Moved", gamepadEvent.GamepadAxis);
			gamepadAxes[gamepadEvent.GamepadAxis] = gamepadEvent.TriggerPosition;
		}
	}
	#endregion

	#region Input mappers
	public void MapMouseButton(string buttonName, MouseButton mouseButton)
	{
		if (!registeredButtons.ContainsKey(buttonName))
			registeredButtons[buttonName] = new ButtonInputData();

		registeredButtons[buttonName].IsMouseRegistered = true;
		registeredButtons[buttonName].MouseButton = mouseButton;
	}
	public void MapKeyboardButton(string buttonName, KeyboardButton keyboardButton)
	{
		if (!registeredButtons.ContainsKey(buttonName))
			registeredButtons[buttonName] = new ButtonInputData();

		registeredButtons[buttonName].IsKeyboardRegistered = true;
		registeredButtons[buttonName].KeyboardButton = keyboardButton;
	}
	public void MapGamepadButton(string buttonName, GamepadButton gamepadButton)
	{
		if (!registeredButtons.ContainsKey(buttonName))
			registeredButtons[buttonName] = new ButtonInputData();

		registeredButtons[buttonName].IsGamepadRegistered = true;
		registeredButtons[buttonName].GamepadButton = gamepadButton;
	}

	public void MapMouseAxis(string axisName, MouseAxis axis, float multiplier, float offset)
	{
		if (!registeredAxes.ContainsKey(axisName))
			registeredAxes[axisName] = new AxisInputData();

		registeredAxes[axisName].IsMouseRegistered = true;
		registeredAxes[axisName].MouseAxis = axis;
		registeredAxes[axisName].MouseAxisMultiplier = multiplier;
		registeredAxes[axisName].MouseAxisOffset = offset;
	}
	public void MapKeyboardAxis(string axisName, KeyboardButton positive, KeyboardButton negative, float multiplier, float offset)
	{
		if (!registeredAxes.ContainsKey(axisName))
			registeredAxes[axisName] = new AxisInputData();

		registeredAxes[axisName].IsKeyboardRegistered = true;
		registeredAxes[axisName].KeyboardAxisPositive = positive;
		registeredAxes[axisName].KeyboardAxisNegative = negative;
		registeredAxes[axisName].KeyboardAxisMultiplier = multiplier;
		registeredAxes[axisName].KeyboardAxisOffset = offset;
	}
	public void MapGamepadAxis(string axisName, GamepadAxis axis, float multiplier, float offset)
	{
		if (!registeredAxes.ContainsKey(axisName))
			registeredAxes[axisName] = new AxisInputData();

		registeredAxes[axisName].IsGamepadRegistered = true;
		registeredAxes[axisName].GamepadAxis = axis;
		registeredAxes[axisName].GamepadAxisMultiplier = multiplier;
		registeredAxes[axisName].GamepadAxisOffset = offset;
	}
	#endregion
}