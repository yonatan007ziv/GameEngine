using System.Numerics;

namespace GraphicsEngine.Components.Shared;

internal abstract class RenderingCamera
{
	protected const float FOV = 90;
	protected const float Near = 0.1f;
	protected const float Far = 10000;

	public Matrix4x4 ViewMatrix { get; protected set; }
	public Matrix4x4 ProjectionMatrix { get; protected set; }
}