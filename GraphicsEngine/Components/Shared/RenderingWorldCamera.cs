using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;
using GameEngine.Extensions;
using System.Numerics;

namespace GraphicsEngine.Components.Shared;

internal class RenderingWorldCamera : RenderingCamera
{
	public WorldObject CameraObject { get; }
	public CameraRenderingMask RenderingMask { get; }

	public float Width { get; set; }
	public float Height { get; set; }
	public ViewPort ViewPort { get; private set; }

	public RenderingWorldCamera(WorldObject cameraObject, CameraRenderingMask renderingMask, int width, int height, ViewPort viewPort)
	{
		CameraObject = cameraObject;
		RenderingMask = renderingMask;

		Width = width;
		Height = height;
		ViewPort = viewPort;

		Update();
	}

	public void Update()
	{
		ViewMatrix = Matrix4x4.CreateLookAt(CameraObject.Transform.Position, CameraObject.Transform.Position + CameraObject.Transform.LocalFront, CameraObject.Transform.LocalUp) * Matrix4x4.CreateScale(-1, 1, 1);
		ProjectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegToRad(FOV), Width / Height, Near, Far);
	}
}