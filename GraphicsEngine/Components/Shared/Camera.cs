using System.Numerics;

namespace GraphicsEngine.Components.Shared;

internal abstract class Camera
{
	protected const float FOV = 90;
	protected const float Near = 0.1f;
	protected const float Far = 1000;

	public Matrix4x4 ViewMatrix { get; set; }
	public Matrix4x4 ProjectionMatrix { get; set; }
}