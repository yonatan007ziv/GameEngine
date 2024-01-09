using System.Numerics;

namespace GameEngine.Core.Components;

public class SceneCamera
{
	private Vector2 prevMousePos = Vector2.Zero;
	private int sensitivitySpeed;

	public readonly Transform transform;
	public float Yaw { get; private set; } = -90;
	public float Pitch { get; private set; }

	public SceneCamera(GameObject parent, int sensitivitySpeed)
	{
		transform = parent.Transform;
		this.sensitivitySpeed = sensitivitySpeed;
	}

	private void UpdateMouse(Vector2 mousePos, float deltaTime)
	{
		Vector2 deltaPos = mousePos - prevMousePos;
		prevMousePos = mousePos;

		Yaw += deltaPos.X * sensitivitySpeed * deltaTime;
		Pitch -= deltaPos.Y * sensitivitySpeed * deltaTime;

		if (Pitch > 89)
			Pitch = 89;
		if (Pitch < -89)
			Pitch = -89;
	}
}