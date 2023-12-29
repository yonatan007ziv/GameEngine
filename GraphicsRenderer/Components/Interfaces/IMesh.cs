using GraphicsRenderer.Components.Shared;

namespace GraphicsRenderer.Components.Interfaces;

internal interface IMesh
{
	public void Render(Transform transform, ICamera camera, IShaderProgram shader);
}