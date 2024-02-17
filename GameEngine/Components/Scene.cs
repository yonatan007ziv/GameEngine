using GameEngine.Components.Interfaces;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Buttons;

namespace GameEngine.Components;

public class Scene : IInputMapper, IDisposable
{
	private static Scene? _loadedScene;

	private bool _loaded;

	protected List<(GameComponent camera, ViewPort viewPort)> cameras = new List<(GameComponent camera, ViewPort viewPort)>();
	protected List<GameObject> gameObjects = new List<GameObject>();

	public async void LoadScene()
	{
		await Services.Implementations.GameEngine.EngineContext.EngineLoadingTask.Task;

		if (_loadedScene is not null)
			_loadedScene.UnloadScene();

		foreach (GameObject gameObject in gameObjects)
			Services.Implementations.GameEngine.EngineContext.AddGameObject(gameObject);
		foreach ((GameComponent camera, ViewPort viewPort) in cameras)
			Services.Implementations.GameEngine.EngineContext.AddCamera(camera, viewPort);

		_loaded = true;
		_loadedScene = this;
	}

	public void UnloadScene()
	{
		foreach (GameObject gameObject in gameObjects)
			Services.Implementations.GameEngine.EngineContext.RemoveGameObject(gameObject);
		foreach ((GameComponent camera, _) in cameras)
			Services.Implementations.GameEngine.EngineContext.RemoveCamera(camera);

		_loaded = false;
		_loadedScene = null;
	}

	public void Dispose()
	{
		if (_loaded)
			UnloadScene();

		foreach (GameObject gameObject in gameObjects)
			gameObject.Dispose();
		foreach ((GameComponent camera, _) in cameras)
			camera.Dispose();
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