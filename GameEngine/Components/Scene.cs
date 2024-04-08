using GameEngine.Components.Interfaces;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Buttons;
using GameEngine.Core.Components.Objects;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace GameEngine.Components;

public class Scene : IInputMapper, IDisposable
{
	public static Scene? LoadedScene { get; private set; } = null;

	private bool _isLoaded;
	public ObservableCollection<WorldObject> WorldObjects { get; } = new ObservableCollection<WorldObject>();
	public ObservableCollection<(WorldCamera worldCamera, ViewPort viewPort)> WorldCameras { get; } = new ObservableCollection<(WorldCamera caworldCameramera, ViewPort viewPort)>();

	public ObservableCollection<UIObject> UIObjects { get; } = new ObservableCollection<UIObject>();
	public ObservableCollection<(UICamera uiCamera, ViewPort viewPort)> UICameras { get; } = new ObservableCollection<(UICamera uiCamera, ViewPort viewPort)>();

	public static ObservableCollection<WorldObject> WorldObjectsQueue { get; } = new ObservableCollection<WorldObject>();
	public static ObservableCollection<(WorldCamera worldCamera, ViewPort viewPort)> WorldCamerasQueue { get; } = new ObservableCollection<(WorldCamera caworldCameramera, ViewPort viewPort)>();

	public static ObservableCollection<UIObject> UIObjectsQueue { get; } = new ObservableCollection<UIObject>();
	public static ObservableCollection<(UICamera uiCamera, ViewPort viewPort)> UICamerasQueue { get; } = new ObservableCollection<(UICamera uiCamera, ViewPort viewPort)>();

	public Scene()
	{
		WorldObjects.CollectionChanged += WorldObjectsChanged;
		WorldCameras.CollectionChanged += WorldCamerasChanged;
		UIObjects.CollectionChanged += UIObjectsChanged;
		UICameras.CollectionChanged += UICamerasChanged;
	}

	private void WorldObjectsChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		if (_isLoaded && e.Action == NotifyCollectionChangedAction.Add)
			foreach (object obj in e.NewItems!)
				if (obj is WorldObject worldObject)
					Services.Implementations.GameEngine.EngineContext.AddWorldObject(worldObject);

		if (_isLoaded && e.Action == NotifyCollectionChangedAction.Remove)
			foreach (object obj in e.OldItems!)
				if (obj is WorldObject worldObject)
					Services.Implementations.GameEngine.EngineContext.RemoveWorldObject(worldObject);
	}
	private void WorldCamerasChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		if (_isLoaded && e.Action == NotifyCollectionChangedAction.Add)
			foreach (object obj in e.NewItems!)
				if (obj is (WorldCamera worldCamera, ViewPort viewPort))
					Services.Implementations.GameEngine.EngineContext.AddWorldCamera(worldCamera, worldCamera.RenderingMaskTags, viewPort);

		if (_isLoaded && e.Action == NotifyCollectionChangedAction.Remove)
			foreach (object obj in e.OldItems!)
				if (obj is (WorldCamera worldCamera, _))
					Services.Implementations.GameEngine.EngineContext.RemoveWorldCamera(worldCamera);
	}
	private void UIObjectsChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		if (_isLoaded && e.Action == NotifyCollectionChangedAction.Add)
			foreach (object obj in e.NewItems!)
				if (obj is UIObject uiObject)
					Services.Implementations.GameEngine.EngineContext.AddUIObject(uiObject);

		if (_isLoaded && e.Action == NotifyCollectionChangedAction.Remove)
			foreach (object obj in e.OldItems!)
				if (obj is UIObject uiObject)
					Services.Implementations.GameEngine.EngineContext.RemoveUIObject(uiObject);
	}
	private void UICamerasChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		if (_isLoaded && e.Action == NotifyCollectionChangedAction.Add)
			foreach (object obj in e.NewItems!)
				if (obj is (UICamera uiCamera, ViewPort viewPort))
					Services.Implementations.GameEngine.EngineContext.AddUICamera(uiCamera, uiCamera.RenderingMaskTags, viewPort);

		if (_isLoaded && e.Action == NotifyCollectionChangedAction.Remove)
			foreach (object obj in e.OldItems!)
				if (obj is (UICamera uiCamera, _))
					Services.Implementations.GameEngine.EngineContext.RemoveUICamera(uiCamera);
	}

	public async void LoadScene()
	{
		await Services.Implementations.GameEngine.EngineContext.EngineLoadingTask.Task;

		if (_isLoaded)
			return;

		LoadedScene?.UnloadScene();

		for (int i = 0; i < WorldObjects.Count; i++)
			Services.Implementations.GameEngine.EngineContext.AddWorldObject(WorldObjects[i]);
		for (int i = 0; i < WorldCameras.Count; i++)
			Services.Implementations.GameEngine.EngineContext.AddWorldCamera(WorldCameras[i].worldCamera, WorldCameras[i].worldCamera.RenderingMaskTags, WorldCameras[i].viewPort);

		for (int i = 0; i < UIObjects.Count; i++)
			Services.Implementations.GameEngine.EngineContext.AddUIObject(UIObjects[i]);
		for (int i = 0; i < UICameras.Count; i++)
			Services.Implementations.GameEngine.EngineContext.AddUICamera(UICameras[i].uiCamera, UICameras[i].uiCamera.RenderingMaskTags, UICameras[i].viewPort);

		for (int i = 0; i < WorldObjectsQueue.Count; i++)
			Services.Implementations.GameEngine.EngineContext.AddWorldObject(WorldObjectsQueue[i]);
		for (int i = 0; i < WorldCamerasQueue.Count; i++)
			Services.Implementations.GameEngine.EngineContext.AddWorldCamera(WorldCamerasQueue[i].worldCamera, WorldCamerasQueue[i].worldCamera.RenderingMaskTags, WorldCamerasQueue[i].viewPort);

		for (int i = 0; i < UIObjectsQueue.Count; i++)
			Services.Implementations.GameEngine.EngineContext.AddUIObject(UIObjectsQueue[i]);
		for (int i = 0; i < UICamerasQueue.Count; i++)
			Services.Implementations.GameEngine.EngineContext.AddUICamera(UICamerasQueue[i].uiCamera, UICamerasQueue[i].uiCamera.RenderingMaskTags, UICamerasQueue[i].viewPort);

		// Clear queues
		WorldObjectsQueue.Clear();
		WorldCamerasQueue.Clear();
		UIObjectsQueue.Clear();
		UICamerasQueue.Clear();

		_isLoaded = true;
		LoadedScene = this;
	}

	public void UnloadScene()
	{
		if (!_isLoaded)
			return;

		for (int i = 0; i < WorldObjects.Count; i++)
			Services.Implementations.GameEngine.EngineContext.RemoveWorldObject(WorldObjects[i]);
		for (int i = 0; i < WorldCameras.Count; i++)
			Services.Implementations.GameEngine.EngineContext.RemoveWorldCamera(WorldCameras[i].worldCamera);

		for (int i = 0; i < UIObjects.Count; i++)
			Services.Implementations.GameEngine.EngineContext.RemoveUIObject(UIObjects[i]);
		for (int i = 0; i < UICameras.Count; i++)
			Services.Implementations.GameEngine.EngineContext.RemoveUICamera(UICameras[i].uiCamera);

		_isLoaded = false;
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