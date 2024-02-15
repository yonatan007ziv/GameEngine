using GameEngine.Components.Interfaces;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Buttons;

namespace GameEngine.Components;

public class Scene : IInputMapper, IDisposable
{
	private bool _loaded;

	protected List<(GameObject camera, ViewPort viewPort)> cameras = new List<(GameObject camera, ViewPort viewPort)>();
	protected List<GameObject> gameObjects = new List<GameObject>();

	public async void LoadScene()
	{
		await Services.Implementations.GameEngine.EngineContext.EngineLoadingTask.Task;

		_loaded = true;
		foreach ((GameObject camera, ViewPort viewPort) in cameras)
			Services.Implementations.GameEngine.EngineContext.AddCamera(camera, viewPort);
		foreach (GameObject gameObject in gameObjects)
			Services.Implementations.GameEngine.EngineContext.AddGameObject(gameObject);
	}

	public void UnloadScene()
	{
		_loaded = false;
		foreach ((GameObject camera, _) in cameras)
			Services.Implementations.GameEngine.EngineContext.RemoveCamera(camera);
		foreach (GameObject gameObject in gameObjects)
			Services.Implementations.GameEngine.EngineContext.RemoveGameObject(gameObject);
	}

	public void Dispose()
	{
		if (_loaded)
			UnloadScene();
		foreach ((GameObject camera, _) in cameras)
			camera.Dispose();
		foreach (GameObject gameObject in gameObjects)
			gameObject.Dispose();
	}

	public void MapMouseButton(string buttonName, MouseButton mouseButton)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.MapMouseButton(buttonName, mouseButton);

	public void MapKeyboardButton(string buttonName, KeyboardButton keyboardButton)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.MapKeyboardButton(buttonName, keyboardButton);

	public void MapGamepadButton(string buttonName, GamepadButton gamepadButton)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.MapGamepadButton(buttonName, gamepadButton);

	public void MapMouseAxis(string axis, MouseAxis movementType, float multiplier, float offset)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.MapMouseAxis(axis, movementType, multiplier, offset);

	public void MapKeyboardAxis(string axis, KeyboardButton positive, KeyboardButton negative, float multiplier, float offset)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.MapKeyboardAxis(axis, positive, negative, multiplier, offset);

	public void MapGamepadAxis(string axis, GamepadAxis analog, float multiplier, float offset)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.MapGamepadAxis(axis, analog, multiplier, offset);
}