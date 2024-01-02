using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Services.Interfaces.InputProviders;
using System.Numerics;

namespace GraphicsRenderer.Components.Shared;

public class FreeCameraController : ICamera
{
	private readonly Camera camera;
	private readonly FreeMovementController movementController;

	public float Width { get => camera.Width; set { camera.Width = value; } }
	public float Height { get => camera.Height; set { camera.Height = value; } }
	public int SensitivitySpeed => camera.SensitivitySpeed;
	public Matrix4x4 ViewMatrix => camera.ViewMatrix;
	public Matrix4x4 ProjectionMatrix => camera.ProjectionMatrix;

	public FreeCameraController(IInputProvider inputProvider, GameObject parent, int sensitivitySpeed, int width, int height)
	{
		camera = new Camera(inputProvider, parent, sensitivitySpeed, width, height);
		movementController = new FreeMovementController(parent, inputProvider);
	}

	public void Update(float deltaTime)
	{
		movementController.UpdateInput(deltaTime);
		camera.Update(deltaTime);
	}
}