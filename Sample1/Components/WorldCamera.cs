using GameEngine.Components.Objects;
using GameEngine.Components.Objects.Scriptable;
using GameEngine.Core.Components;
using System.Numerics;

namespace Sample1.Components;

internal class WorldCamera : ScriptableWorldComponent
{
	private const float sensitivity = 25;

	private readonly AxesSet cameraAxes;

	public WorldCamera(WorldObject parent, AxesSet cameraAxes)
		: base(parent)
	{
		this.cameraAxes = cameraAxes;

		Meshes.Add(new MeshData("Camera.obj", "Red.mat"));
	}

	public override void Update(float deltaTime)
	{
		if (MouseLocked)
		{
			Vector2 cameraVector = new Vector2(GetAxis(cameraAxes.Horizontal), GetAxis(cameraAxes.Vertical));

			// Clamp camera
			float clampedX = Math.Clamp(Transform.Rotation.X + cameraVector.Y * deltaTime * sensitivity, -90, 90); // Clamp vertical input to -1 and 1
			Transform.Rotation = new Vector3(clampedX, Transform.Rotation.Y - cameraVector.X * deltaTime * sensitivity, 0);
		}
	}
}