using GameEngine.Core.Components;
using GameEngine.Core.Extensions;
using GraphicsEngine.Components.Interfaces;
using System.Numerics;

namespace GraphicsEngine.Components.Shared;

internal class Camera : IObject
{
	private const float FOV = 90;
	private const float Near = 0.1f;
	private const float Far = 1000;

	private readonly RenderedObject renderedObject;

	public int Id { get; }

	public bool UI { get; set; }
	public Transform Transform { get; set; }
	public float Width { get; set; }
	public float Height { get; set; }
	public Matrix4x4 ViewMatrix { get; private set; }
	public Matrix4x4 ProjectionMatrix { get; private set; }
	public ViewPort ViewPort { get; private set; }

	public List<IMeshRenderer> Meshes => renderedObject.Meshes;

	public Camera(int id, Transform transform, int width, int height, ViewPort viewPort)
	{
		Id = id;
		Transform = transform;

		Width = width;
		Height = height;
		ViewPort = viewPort;

		renderedObject = new RenderedObject(id, transform);
	}

	public void Render(Camera camera)
	{
		renderedObject.Render(camera);
	}

	public void Update()
	{
		renderedObject.Update();
		ViewMatrix = Matrix4x4.CreateLookAt(Transform.Position, Transform.Position + Transform.LocalFront, Transform.LocalUp);
		if (UI)
			ProjectionMatrix = Matrix4x4.CreateOrthographicOffCenter(-1, 1, -1, 1, Near, Far);
		else
			ProjectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegToRad(FOV), Width / Height, Near, Far);
	}
}