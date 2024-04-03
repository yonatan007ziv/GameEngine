using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;
using System.Numerics;

namespace GraphicsEngine.Components.Shared;

internal class RenderingUICamera : RenderingCamera
{
	public UIObject CameraObject { get; }
	public CameraRenderingMask RenderingMask { get; }

	public float Width { get; set; }
	public float Height { get; set; }
	public ViewPort ViewPort { get; private set; }

	public RenderingUICamera(UIObject cameraObject, CameraRenderingMask renderingMask, int width, int height, ViewPort viewPort)
	{
		CameraObject = cameraObject;
		RenderingMask = renderingMask;

		Width = width;
		Height = height;
		ViewPort = viewPort;

		ViewMatrix = Matrix4x4.CreateLookAt(CameraObject.Transform.Position, CameraObject.Transform.Position - CameraObject.Transform.LocalFront, CameraObject.Transform.LocalUp);
		ProjectionMatrix = Matrix4x4.CreateOrthographicOffCenter(-1, 1, -1, 1, Near, Far);
	}
}