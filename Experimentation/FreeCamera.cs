using GameEngine.Components.Objects.Scriptable;
using System.Numerics;

namespace Experimentation;

internal class FreeCamera : ScriptableWorldObject
{
	public readonly WorldCamera camera;

	public FreeCamera()
	{
		Transform.Position = new Vector3(0, 10, 0);
		camera = new WorldCamera(this);
	}

	public override void Update(float deltaTime)
	{
		Transform.Position += (Transform.LocalRight * GetAxis("KeyboardX") + Transform.LocalFront * GetAxis("KeyboardY")) * deltaTime * 10;
		Transform.Rotation += new Vector3(GetAxis("MouseY"), GetAxis("MouseX"), 0) * deltaTime;

		Console.WriteLine(Transform.Rotation);
	}
}