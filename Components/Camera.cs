using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenGLRenderer.Components;

internal class Camera
{
	private readonly GameObject parent;

	public Matrix4 ViewMatrix { get; private set; }
	public Matrix4 ProjectionMatrix { get; private set; }

	private readonly Vector3 right = Vector3.UnitX;
	private readonly Vector3 up = Vector3.UnitY;
	private readonly Vector3 front = -Vector3.UnitZ;

	private readonly int sensitivitySpeed;
	private readonly int movementSpeed;

	private readonly float width;
	private readonly float height;

	public Camera(GameObject parent, int sensitivitySpeed, int movementSpeed, float width, float height)
    {
		this.parent = parent;
		this.sensitivitySpeed = sensitivitySpeed;
		this.movementSpeed = movementSpeed;
		this.width = width;
		this.height = height;
	}

	private void UpdateMouse(MouseState mouseState, float deltaTime)
	{

	}

	private void UpdateKeyboard(KeyboardState keyboardState, float deltaTime)
	{
		if (keyboardState.IsKeyDown(Keys.W))
			parent.Position += front * movementSpeed * deltaTime;

		if (keyboardState.IsKeyDown(Keys.A))
			parent.Position -= right * movementSpeed * deltaTime;

		if (keyboardState.IsKeyDown(Keys.S))
			parent.Position -= front * movementSpeed * deltaTime;

		if (keyboardState.IsKeyDown(Keys.D))
			parent.Position += right * movementSpeed * deltaTime;
	}

	private void UpdateMatrices()
	{
		ViewMatrix = Matrix4.LookAt(parent.Position, parent.Position + front, up);
		ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90), width / height, 0.1f, 25);
	}

	public void Update(MouseState mouseState, KeyboardState keyboardState, FrameEventArgs args)
	{
		float deltaTime = (float)args.Time;

		UpdateMouse(mouseState, deltaTime);
		UpdateKeyboard(keyboardState, deltaTime);
		UpdateMatrices();
    }
}