using GameEngine.Core.Components;
using GameEngine.Core.Components.CommunicationComponentsData;
using GraphicsEngine.Services.Interfaces.InputProviders;
using System.Numerics;

namespace GraphicsEngine.Components.Shared;

public class FreeCameraController
{
	private readonly RenderedCamera camera;
	private readonly FreeMovementController movementController;

	public float Width { get => camera.Width; set { camera.Width = value; } }
	public float Height { get => camera.Height; set { camera.Height = value; } }
	public Matrix4x4 ViewMatrix => camera.ViewMatrix;
	public Matrix4x4 ProjectionMatrix => camera.ProjectionMatrix;

	public FreeCameraController(IInputProvider inputProvider, GameObjectData parent, int width, int height)
	{
		camera = new RenderedCamera(new Transform(parent.Transform), width, height);
		movementController = new FreeMovementController(new Transform(parent.Transform), inputProvider);
	}

	public void Update(float deltaTime)
	{
		movementController.UpdateInput(deltaTime);
		camera.Update();
	}
}