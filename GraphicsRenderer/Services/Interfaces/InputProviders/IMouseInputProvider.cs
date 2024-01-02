using System.Numerics;

namespace GraphicsRenderer.Services.Interfaces.InputProviders;

public interface IMouseInputProvider
{
	Vector2 MousePosition { get; }
}