using System.Numerics;

namespace GraphicsEngine.Services.Interfaces.InputProviders;

public interface IMouseInputProvider
{
	Vector2 MousePosition { get; }
}