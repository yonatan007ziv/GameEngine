using GameEngine.Core.Components;
using GameEngine.Core.Extensions;
using System.Numerics;

namespace GraphicsRenderer.Components.Shared;

public class RendererCamera
{
	public Transform Transform { get; }

	private float yaw = -90, pitch;

	public float Width { get; set; }
	public float Height { get; set; }
	public Matrix4x4 ViewMatrix { get; private set; }
	public Matrix4x4 ProjectionMatrix { get; private set; }

	public RendererCamera(Transform transform, int width, int height)
	{
		Width = width;
		Height = height;

		Transform = transform;
	}

	private void UpdateVectors()
	{
		Transform.Rotation = new Vector3(0, MathHelper.DegToRad(-yaw), 0);

		float pitchRadians = MathHelper.DegToRad(pitch);
		float yawRadians = MathHelper.DegToRad(yaw);
		Transform.LocalFront = new Vector3(MathF.Cos(pitchRadians) * MathF.Cos(yawRadians), MathF.Sin(pitchRadians), MathF.Cos(pitchRadians) * MathF.Sin(yawRadians));

		Transform.LocalFront = Vector3.Normalize(Transform.LocalFront);
		Transform.LocalRight = Vector3.Normalize(Vector3.Cross(Transform.LocalFront, Vector3.UnitY));
		Transform.LocalUp = Vector3.Normalize(Vector3.Cross(Transform.LocalRight, Transform.LocalFront));

		ViewMatrix = Matrix4x4.CreateLookAt(Transform.Position, (Transform.Position + Transform.LocalFront), Transform.LocalUp);
		ProjectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegToRad(90), Width / Height, 0.1f, 10000);
	}

	public void Update()
		=> UpdateVectors();
}