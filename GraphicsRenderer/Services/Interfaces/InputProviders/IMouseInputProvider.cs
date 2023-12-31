using System.Numerics;

namespace GraphicsRenderer.Services.Interfaces.InputProviders;

internal interface IMouseInputProvider
{
	Vector2 MousePosition { get; }
}