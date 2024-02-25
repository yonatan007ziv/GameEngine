using GameEngine.Components.Interfaces;
using GameEngine.Components.Objects;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Buttons;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace GameEngine.Components;

public class Scene : IInputMapper, IDisposable
{
	public static Scene LoadedScene { get; private set; } = null!;

	private bool _loaded;
	public readonly ObservableCollection<(WorldComponent camera, ViewPort viewPort)> worldCameras = new ObservableCollection<(WorldComponent camera, ViewPort viewPort)>();
	public readonly ObservableCollection<WorldObject> worldObjects = new ObservableCollection<WorldObject>();

	public readonly ObservableCollection<(UIComponent camera, ViewPort viewPort)> uiCameras = new ObservableCollection<(UIComponent camera, ViewPort viewPort)>();
	public readonly ObservableCollection<UIObject> uiObjects = new ObservableCollection<UIObject>();

	public Scene()
	{
		worldCameras.CollectionChanged += WorldCamerasChanged;
		worldObjects.CollectionChanged += WorldObjectsChanged;
		uiCameras.CollectionChanged += UICamerasChanged;
		uiObjects.CollectionChanged += UIObjectsChanged;
	}

	private void WorldCamerasChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		if (_loaded && e.Action == NotifyCollectionChangedAction.Remove)
			foreach (object obj in e.OldItems!)
				if (obj is WorldComponent worldCamera)
					Services.Implementations.GameEngine.EngineContext.RemoveWorldCamera(worldCamera);
	}
	private void WorldObjectsChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		if (_loaded && e.Action == NotifyCollectionChangedAction.Remove)
			foreach (object obj in e.OldItems!)
				if (obj is WorldObject worldObject)
					Services.Implementations.GameEngine.EngineContext.RemoveWorldObject(worldObject);
	}
	private void UICamerasChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		if (_loaded && e.Action == NotifyCollectionChangedAction.Remove)
			foreach (object obj in e.OldItems!)
				if (obj is UIComponent uiCamera)
					Services.Implementations.GameEngine.EngineContext.RemoveUICamera(uiCamera);
	}
	private void UIObjectsChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		if (_loaded && e.Action == NotifyCollectionChangedAction.Remove)
			foreach (object obj in e.OldItems!)
				if (obj is UIObject uiObject)
					Services.Implementations.GameEngine.EngineContext.RemoveUIObject(uiObject);
	}

	public async void LoadScene()
	{
		await Services.Implementations.GameEngine.EngineContext.EngineLoadingTask.Task;

		if (_loaded)
			return;

		LoadedScene?.UnloadScene();

		foreach (WorldObject worldObject in worldObjects)
			Services.Implementations.GameEngine.EngineContext.AddWorldObject(worldObject);
		foreach ((WorldComponent camera, ViewPort viewPort) in worldCameras)
			Services.Implementations.GameEngine.EngineContext.AddWorldCamera(camera, viewPort);

		foreach (UIObject uiObject in uiObjects)
			Services.Implementations.GameEngine.EngineContext.AddUIObject(uiObject);
		foreach ((UIComponent camera, ViewPort viewPort) in uiCameras)
			Services.Implementations.GameEngine.EngineContext.AddUICamera(camera, viewPort);

		_loaded = true;
		LoadedScene = this;
	}

	public void UnloadScene()
	{
		if (!_loaded)
			return;

		foreach (WorldObject gameObject in worldObjects)
			Services.Implementations.GameEngine.EngineContext.RemoveWorldObject(gameObject);
		foreach ((WorldComponent camera, _) in worldCameras)
			Services.Implementations.GameEngine.EngineContext.RemoveWorldCamera(camera);

		_loaded = false;
		LoadedScene = null!;
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

	#region Dispose pattern
	private bool disposedValue;
	~Scene()
	{
		Dispose(disposing: false);
	}
	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{

			}

			UnloadScene();
			disposedValue = true;
		}
	}
	#endregion
}