using GraphicsRenderer.Components.Extensions;
using GraphicsRenderer.Services.Interfaces.InputProviders;
using System.Numerics;

namespace GraphicsRenderer.Components.Shared;

public class Camera
{
	private IInputProvider inputProvider;
	private readonly Transform parentTransform;

	private Vector2 prevMousePos = Vector2.Zero;

	private float yaw = -90, pitch;

	public float Width { get; set; }
	public float Height { get; set; }
	public int SensitivitySpeed { get; private set; }
	public Matrix4x4 ViewMatrix { get; private set; }
	public Matrix4x4 ProjectionMatrix { get; private set; }

	public Camera(IInputProvider inputProvider, GameObject parent, int sensitivitySpeed, int width, int height)
	{
		this.inputProvider = inputProvider;

		Width = width;
		Height = height;

		parentTransform = parent.Transform;
		SensitivitySpeed = sensitivitySpeed;
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
		parentTransform.Rotation = new Vector3(0, MathHelper.DegToRad(-yaw), 0);

		float pitchRadians = MathHelper.DegToRad(pitch);
		float yawRadians = MathHelper.DegToRad(yaw);
		parentTransform.LocalFront = new Vector3(MathF.Cos(pitchRadians) * MathF.Cos(yawRadians), MathF.Sin(pitchRadians), MathF.Cos(pitchRadians) * MathF.Sin(yawRadians));

		parentTransform.LocalFront = Vector3.Normalize(parentTransform.LocalFront);
		parentTransform.LocalRight = Vector3.Normalize(Vector3.Cross(parentTransform.LocalFront, Vector3.UnitY));
		parentTransform.LocalUp = Vector3.Normalize(Vector3.Cross(parentTransform.LocalRight, parentTransform.LocalFront));

		ViewMatrix = Matrix4x4.CreateLookAt(parentTransform.Position, (parentTransform.Position + parentTransform.LocalFront), parentTransform.LocalUp);
		ProjectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegToRad(90), Width / Height, 0.1f, 10000);
	}

	public void Update(float deltaTime)
	{
		UpdateMouse(inputProvider.MousePosition, deltaTime);
		UpdateVectors();
	}
}