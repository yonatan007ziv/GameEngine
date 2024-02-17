using GameEngine.Components;
using GameEngine.Core.Components;
using System.Numerics;

namespace Sample1.Components;

internal class Camera : ScriptableGameComponent
{
	private const float sensitivity = 25;

	private readonly AxesSet cameraAxes;

	public Camera(GameObject parent, AxesSet cameraAxes)
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
			if (Math.Abs(Transform.Rotation.X + cameraVector.Y) > 89.5f)
				cameraVector.Y = 0;
			Transform.Rotation += new Vector3(cameraVector.Y, -cameraVector.X, 0) * deltaTime * sensitivity;
		}
	}
}