using GameEngine.Core.Components;
using GraphicsEngine.Services.Interfaces.InputProviders;
using System.Numerics;

namespace GraphicsEngine.Components.Shared;

public class FreeCameraController
{
	private readonly RendererCamera camera;
	private readonly FreeMovementController movementController;

	public float Width { get => camera.Width; set { camera.Width = value; } }
	public float Height { get => camera.Height; set { camera.Height = value; } }
	public Matrix4x4 ViewMatrix => camera.ViewMatrix;
	public Matrix4x4 ProjectionMatrix => camera.ProjectionMatrix;

	public FreeCameraController(IInputProvider inputProvider, GameObject parent, int width, int height)
	{
		camera = new RendererCamera(parent.Transform, width, height);
		movementController = new FreeMovementController(parent, inputProvider);
	}

	public void Update(float deltaTime)
	{
		movementController.UpdateInput(deltaTime);
		camera.Update();
	}
}