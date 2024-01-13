using GameEngine.Core.Components;
using GameEngine.Core.Extensions;
using System.Numerics;

namespace GraphicsEngine.Components.Shared;

internal class RenderedCamera
{
	public Transform Transform { get; set; }
	public float Width { get; set; }
	public float Height { get; set; }
	public Matrix4x4 ViewMatrix { get; private set; }
	public Matrix4x4 ProjectionMatrix { get; private set; }

	public RenderedCamera(Transform parentTransform, int width, int height)
	{
		Width = width;
		Height = height;

		Transform = parentTransform;
	}

	private void UpdateVectors()
	{
		ViewMatrix = Matrix4x4.CreateLookAt(Transform.Position, Transform.Position + Transform.LocalFront, Transform.LocalUp);
		ProjectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegToRad(90), Width / Height, 0.1f, 10000);
	}

	public void Update()
		=> UpdateVectors();
}