﻿using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Buttons;
using GameEngine.Core.Components.Objects;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;

namespace GameEngine.Components.ScriptableObjects;

public abstract class ScriptableWorldObject : WorldObject
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

	public ScriptableWorldObject() { }
	public ScriptableWorldObject(WorldObject parent)
		: base(parent) { }

	// Gets all world objects within a certain distance from the current one
	public List<WorldObject> GetWorldObjectsWithinOriginDistance(float distance)
	{
		List<int> ids = Services.Implementations.GameEngine.EngineContext.PhysicsEngine.GetObjectIdsWithinDistance(Transform.Position, distance);

		List<WorldObject> objects = new List<WorldObject>(ids.Count);
		foreach (int id in ids)
			objects.Add(GetWorldObjectFromId(id)!);
		return objects;
	}
	public static WorldObject? GetWorldObjectFromId(int id)
		=> Services.Implementations.GameEngine.EngineContext.GetWorldObjectFromId(id);
	public static UIObject? GetUIObjectFromId(int id)
		=> Services.Implementations.GameEngine.EngineContext.GetUIObjectFromId(id);

	#region Collider info
	public void RaycastHitAll(Vector3 fromPos, Vector3 direction, out List<RaycastHit> hits)
		=> Services.Implementations.GameEngine.EngineContext.PhysicsEngine.RaycastHitAll([Id], fromPos, direction, out hits);
	public bool TouchingColliderTag(string tag)
	{
		int[] touchingIds = GetTouchingColliderIds();
		for (int i = 0; i < touchingIds.Length; i++)
		{
			WorldObject? obj = GetWorldObjectFromId(touchingIds[i]);

			if (obj is null)
				continue;

			if (obj.Tag == tag)
				return true;
		}
		return false;
	}
	public int[] GetTouchingColliderIds()
		=> Services.Implementations.GameEngine.EngineContext.PhysicsEngine.GetTouchingColliderIds(Id);
	#endregion

	#region Input info
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

	public static bool GetKeyboardButtonPressed(KeyboardButton keyboardButton)
	=> Services.Implementations.GameEngine.EngineContext.InputEngine.GetKeyboardButtonPressed(keyboardButton);
	public static bool GetKeyboardButtonDown(KeyboardButton keyboardButton)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.GetKeyboardButtonDown(keyboardButton);

	public static bool GetJoystickButtonPressed(GamepadButton joystickButton)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.GetGamepadButtonPressed(joystickButton);
	public static bool GetJoystickButtonDown(GamepadButton joystickButton)
		=> Services.Implementations.GameEngine.EngineContext.InputEngine.GetGamepadButtonDown(joystickButton);
	#endregion

	public static void SetWindowBackgroundColor(Color color)
		=> Services.Implementations.GameEngine.EngineContext.SetWindowBackgroundColor(color);

	// The scriptable part of the WorldObject
	public abstract void Update(float deltaTime);
}