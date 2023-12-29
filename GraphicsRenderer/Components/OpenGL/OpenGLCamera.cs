using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Components.Shared;
using OpenTK.Mathematics;

namespace GraphicsRenderer.Components.OpenGL;

internal class OpenGLCamera : ICamera
{
	private float yaw, pitch;
	private System.Numerics.Vector2 prevMousePos = System.Numerics.Vector2.Zero;
	private Vector3 right = Vector3.UnitX;
	private Vector3 up = Vector3.UnitY;
	private Vector3 front = -Vector3.UnitZ;

	public int Width { get; set; }
	public int Height { get; set; }
	public GameObject Parent { get; set; }
	public int SensitivitySpeed { get; set; }
	public int MovementSpeed { get; set; }

	public Matrix4 ViewMatrix { get; private set; }
	public Matrix4 ProjectionMatrix { get; private set; }

	public OpenGLCamera(GameObject parent, int sensitivitySpeed, int movementSpeed, int width, int height)
	{
		Parent = parent;
		SensitivitySpeed = sensitivitySpeed;
		MovementSpeed = movementSpeed;
		Width = width;
		Height = height;
	}

	private void UpdateMouse(System.Numerics.Vector2 mousePos, float deltaTime)
	{
		System.Numerics.Vector2 deltaPos = mousePos - prevMousePos;
		prevMousePos = mousePos;

		yaw += deltaPos.X * deltaTime * SensitivitySpeed;
		pitch -= deltaPos.Y * deltaTime * SensitivitySpeed;

		if (pitch > 89)
			pitch = 89;
		if (pitch < -89)
			pitch = -89;
	}

	// Player Controller Code
	/* 
	private void UpdateKeyboard(KeyboardState keyboardState, float deltaTime)
	{
		float movementSpeed = MovementSpeed * (keyboardState.IsKeyDown(Keys.LeftShift) ? 2.5f : 1);

		if (keyboardState.IsKeyDown(Keys.W))
			Parent.Transform.Position += front * movementSpeed * deltaTime;

		if (keyboardState.IsKeyDown(Keys.A))
			Parent.Transform.Position -= right * movementSpeed * deltaTime;

		if (keyboardState.IsKeyDown(Keys.S))
			Parent.Transform.Position -= front * movementSpeed * deltaTime;

		if (keyboardState.IsKeyDown(Keys.D))
			Parent.Transform.Position += right * movementSpeed * deltaTime;

		if (keyboardState.IsKeyDown(Keys.E))
			Parent.Transform.Position += up * movementSpeed * deltaTime;

		if (keyboardState.IsKeyDown(Keys.Q))
			Parent.Transform.Position -= up * movementSpeed * deltaTime;
	}
	*/

	private void UpdateVectors()
	{
		Parent.Transform.Rotation = new Vector3(0, MathHelper.DegreesToRadians(-yaw), 0);

		front.X = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Cos(MathHelper.DegreesToRadians(yaw));
		front.Y = MathF.Sin(MathHelper.DegreesToRadians(pitch));
		front.Z = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Sin(MathHelper.DegreesToRadians(yaw));

		front = Vector3.Normalize(front);
		right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
		up = Vector3.Normalize(Vector3.Cross(right, front));

		ViewMatrix = Matrix4.LookAt(Parent.Transform.Position, Parent.Transform.Position + front, up);
		ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90), (float)Width / Height, 0.1f, 10000);
	}

	public void Update(System.Numerics.Vector2 mousePos, float deltaTime)
	{
		UpdateMouse(mousePos, deltaTime);
		UpdateVectors();
	}
}