using GameEngine.Components.Interfaces;
using GameEngine.Components.Objects;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Buttons;

namespace GameEngine.Components;

public class Scene : IInputMapper, IDisposable
{
	private static Scene? _loadedScene;

	private bool _loaded;

	protected List<(WorldComponent camera, ViewPort viewPort)> worldCameras = new List<(WorldComponent camera, ViewPort viewPort)>();
	protected List<WorldObject> worldObjects = new List<WorldObject>();

	protected List<(UIComponent camera, ViewPort viewPort)> uiCameras = new List<(UIComponent camera, ViewPort viewPort)>();
	protected List<UIObject> uiObjects = new List<UIObject>();

	public async void LoadScene()
	{
		await Services.Implementations.GameEngine.EngineContext.EngineLoadingTask.Task;

		if (_loadedScene is not null)
			_loadedScene.UnloadScene();

		foreach (WorldObject worldObject in worldObjects)
			Services.Implementations.GameEngine.EngineContext.AddWorldObject(worldObject);
		foreach ((WorldComponent camera, ViewPort viewPort) in worldCameras)
			Services.Implementations.GameEngine.EngineContext.AddWorldCamera(camera, viewPort);

		foreach (UIObject uiObject in uiObjects)
			Services.Implementations.GameEngine.EngineContext.AddUIObject(uiObject);
		foreach ((UIComponent camera, ViewPort viewPort) in uiCameras)
			Services.Implementations.GameEngine.EngineContext.AddUICamera(camera, viewPort);

		_loaded = true;
		_loadedScene = this;
	}

	public void UnloadScene()
	{
		foreach (WorldObject gameObject in worldObjects)
			Services.Implementations.GameEngine.EngineContext.RemoveWorldObject(gameObject);
		foreach ((WorldComponent camera, _) in worldCameras)
			Services.Implementations.GameEngine.EngineContext.RemoveWorldCamera(camera);

		_loaded = false;
		_loadedScene = null;
	}

	public void Dispose()
	{
		if (_loaded)
			UnloadScene();

		foreach (WorldObject gameObject in worldObjects)
			gameObject.Dispose();
		foreach ((WorldComponent camera, _) in worldCameras)
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