using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;
using GameEngine.Extensions;
using System.Numerics;

namespace GraphicsEngine.Components.Shared;

internal class WorldRenderingCamera : RenderingCamera
{
	public WorldObject CameraObject { get; }
	public CameraRenderingMask RenderingMask { get; }

	public float Width { get; set; }
	public float Height { get; set; }
	public ViewPort ViewPort { get; private set; }

	public WorldRenderingCamera(WorldObject cameraObject, CameraRenderingMask renderingMask, int width, int height, ViewPort viewPort)
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
		ViewMatrix = Matrix4x4.CreateLookAt(CameraObject.Transform.Position, CameraObject.Transform.Position + CameraObject.Transform.LocalFront, CameraObject.Transform.LocalUp);
		ProjectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegToRad(FOV), Width / Height, Near, Far);
	}
}