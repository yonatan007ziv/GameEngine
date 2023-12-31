using GraphicsRenderer.Components.Extensions;
using GraphicsRenderer.Services.Implementations.OpenGL.Renderer;
using GraphicsRenderer.Services.Interfaces.InputProviders;
using System.Numerics;

namespace GraphicsRenderer.Services.Implementations.OpenGL.Input;

internal class OpenGLMouseInputProvider : IMouseInputProvider
{
	public Vector2 MousePosition => OpenGLRenderer.Instance.MousePosition.ToNumerics();
}