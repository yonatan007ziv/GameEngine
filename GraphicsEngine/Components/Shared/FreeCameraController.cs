using GameEngine.Core.Components;
using System.Numerics;

namespace GraphicsEngine.Components.Shared;

public class FreeCameraController
{
	private readonly Camera camera;
	// private readonly FreeMovementController movementController;

	public float Width { get => camera.Width; set { camera.Width = value; } }
	public float Height { get => camera.Height; set { camera.Height = value; } }
	public Matrix4x4 ViewMatrix => camera.ViewMatrix;
	public Matrix4x4 ProjectionMatrix => camera.ProjectionMatrix;

	public FreeCameraController(GameObjectData parent, int width, int height)
	{
		// camera = new Camera(-1, parent.Transform, width, height);
		// movementController = new FreeMovementController(new Transform(parent.Transform), inputProvider);
	}

	public void Update(float deltaTime)
	{
		// movementController.UpdateInput(deltaTime);
		camera.Update();
	}
}