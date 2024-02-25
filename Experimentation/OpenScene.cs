using GameEngine.Components;
using System.Numerics;

namespace Experimentation;

internal class OpenScene : Scene
{
	public OpenScene()
	{
		FreeCamera freeCamera = new FreeCamera();
		worldObjects.Add(freeCamera);
		worldCameras.Add((freeCamera.camera, new GameEngine.Core.Components.ViewPort(0.5f, 0.5f, 1, 1)));

		worldObjects.Add(new Ground(new Vector2(100, 100)));
	}
}