using GraphicsRenderer.Components.Extensions;
using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Services.Interfaces.InputProviders;
using System.Numerics;

namespace GraphicsRenderer.Components.OpenGL;

internal class OpenGLCamera : ICamera
{
	private readonly Transform parentTransform;
	private float yaw = -90, pitch;
	private Vector2 prevMousePos = Vector2.Zero;

	public int Width { get; set; }
	public int Height { get; set; }
	public int SensitivitySpeed { get; set; }

	public OpenTK.Mathematics.Matrix4 ViewMatrix { get; private set; }
	public OpenTK.Mathematics.Matrix4 ProjectionMatrix { get; private set; }

	public OpenGLCamera(GameObject parent, int sensitivitySpeed, int width, int height)
	{
		parentTransform = parent.Transform;
		SensitivitySpeed = sensitivitySpeed;
		Width = width;
		Height = height;
	}

	private void UpdateMouse(Vector2 mousePos, float deltaTime)
	{
		Vector2 deltaPos = mousePos - prevMousePos;
		prevMousePos = mousePos;

		yaw += deltaPos.X * SensitivitySpeed * deltaTime;
		pitch -= deltaPos.Y * SensitivitySpeed * deltaTime;

		if (pitch > 89)
			pitch = 89;
		if (pitch < -89)
			pitch = -89;
	}

	private void UpdateVectors()
	{
		parentTransform.Rotation = new Vector3(0, OpenTK.Mathematics.MathHelper.DegreesToRadians(-yaw), 0);

		float pitchRadians = OpenTK.Mathematics.MathHelper.DegreesToRadians(pitch);
		float yawRadians = OpenTK.Mathematics.MathHelper.DegreesToRadians(yaw);
		parentTransform.Front = new Vector3(MathF.Cos(pitchRadians) * MathF.Cos(yawRadians), MathF.Sin(pitchRadians), MathF.Cos(pitchRadians) * MathF.Sin(yawRadians));

		parentTransform.Front = Vector3.Normalize(parentTransform.Front);
		parentTransform.Right = Vector3.Normalize(Vector3.Cross(parentTransform.Front, Vector3.UnitY));
		parentTransform.Up = Vector3.Normalize(Vector3.Cross(parentTransform.Right, parentTransform.Front));

		ViewMatrix = OpenTK.Mathematics.Matrix4.LookAt(parentTransform.Position.ToOpenTK(), (parentTransform.Position + parentTransform.Front).ToOpenTK(), parentTransform.Up.ToOpenTK());
		ProjectionMatrix = OpenTK.Mathematics.Matrix4.CreatePerspectiveFieldOfView(OpenTK.Mathematics.MathHelper.DegreesToRadians(90), (float)Width / Height, 0.1f, 10000);
	}

	public void Update(IInputProvider inputProvider, float deltaTime)
	{
		UpdateMouse(inputProvider.MousePosition, deltaTime);
		UpdateVectors();
	}
}