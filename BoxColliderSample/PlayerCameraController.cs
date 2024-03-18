using GameEngine.Components;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;
using System.Numerics;

namespace BoxColliderSample;

internal class PlayerCameraController : WorldCamera
{
	private const float sensitivity = 25;
	private const float clampAngleX = 90;

	public PlayerCameraController(WorldObject parent)
		: base(parent)
	{
		Meshes.Add(new MeshData("Camera.obj", "Red.mat"));
	}

	public override void Update(float deltaTime)
	{
		if (MouseLocked)
		{
			Vector2 cameraVector = new Vector2(GetAxis("XCamera"), GetAxis("YCamera"));

			// Clamp camera
			float clampedX = Math.Clamp(Transform.Rotation.X - cameraVector.Y * deltaTime * sensitivity, -clampAngleX, clampAngleX);
			Transform.Rotation = new Vector3(clampedX, Transform.Rotation.Y + cameraVector.X * deltaTime * sensitivity, 0);
		}
	}
}