using GraphicsEngine.Components.Extensions;
using GraphicsEngine.Services.Implementations.OpenGL.Renderer;
using GraphicsEngine.Services.Interfaces.InputProviders;
using System.Numerics;

namespace GraphicsEngine.Services.Implementations.OpenGL.Input;

public class OpenGLMouseInputProvider : IMouseInputProvider
{
	public Vector2 MousePosition => OpenGLRenderer.Instance.MousePosition.ToNumerics();
}