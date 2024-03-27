using GameEngine.Core.Components;
using GameEngine.Core.Components.Fonts;
using GameEngine.Core.Components.Input.Events;
using GameEngine.Core.SharedServices.Interfaces;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Services.Interfaces;
using System.Drawing;
using System.Numerics;

namespace GraphicsEngine.Services.Implementations.Direct12;

internal class Direct12Renderer : IInternalGraphicsRenderer
{
	public IFactory<string, string, MeshRenderer> MeshFactory => throw new NotImplementedException();

	public IntPtr WindowHandle => throw new NotImplementedException();

	public Vector2 WindowSize => throw new NotImplementedException();

	public string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	public bool LogRenderingMessages { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

	public event Action LoadEvent;
	public event Action ResizedEvent;
	public event Action<MouseEventData>? MouseEvent;
	public event Action<KeyboardEventData>? KeyboardEvent;
	public event Action<GamepadEventData>? GamepadEvent;

	public void DrawCharacterGlyf(CharacterGlyf characterGlyf, int fontSize, Vector2 centeredPosition)
	{
		throw new NotImplementedException();
	}

	public void ProcessEvents()
	{
		throw new NotImplementedException();
	}

	public void SetBackgroundColor(Color color)
	{
		throw new NotImplementedException();
	}

	public void SetDepthTest(bool enable)
	{
		throw new NotImplementedException();
	}

	public void SetMouseLocked(bool locked)
	{
		throw new NotImplementedException();
	}

	public void SetMousePosition(Vector2 vector2)
	{
		throw new NotImplementedException();
	}

	public void SetViewport(ViewPort viewport)
	{
		throw new NotImplementedException();
	}

	public void Start()
	{
		throw new NotImplementedException();
	}

	public void SwapBuffers()
	{
		throw new NotImplementedException();
	}
}