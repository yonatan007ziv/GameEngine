using GameEngine.Core.Components;
using System.Numerics;

namespace GraphicsEngine.Components.Shared;

internal class UICamera : Camera
{
	public int Id { get; }

	public Transform Transform { get; set; }
	public float Width { get; set; }
	public float Height { get; set; }
	public ViewPort ViewPort { get; private set; }

	public UICamera(int id, int width, int height, ViewPort viewPort)
	{
		Id = id;
		Transform = new Transform();

		Width = width;
		Height = height;
		ViewPort = viewPort;

		ViewMatrix = Matrix4x4.CreateLookAt(Transform.Position, Transform.Position - Transform.LocalFront, Transform.LocalUp);
		ProjectionMatrix = Matrix4x4.CreateOrthographicOffCenter(-1, 1, -1, 1, Near, Far);
	}
}