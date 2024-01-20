using GameEngine.Core.API;
using GameEngine.Core.Components;

namespace GraphicsEngine;

public class Program
{
	public static void Main()
	{
		IGraphicsEngine renderer = GraphicsEngineProvider.BuildEngine();

		GameObjectData cameraObj = new GameObjectData(0, new Transform() { Position = new System.Numerics.Vector3(0, 75, 0), Rotation = new System.Numerics.Vector3(-90, 0, 0) }, new List<MeshData>());
		renderer.SetCamera(ref cameraObj);
		renderer.UpdateGameObject(ref cameraObj);

		renderer.Start();

		GameObjectData trexObj = new GameObjectData(1, new Transform() { Scale = new System.Numerics.Vector3(1, 1, 1) }, new List<MeshData>() { new MeshData("Trex.obj", "Trex.mat") });
		renderer.UpdateGameObject(ref trexObj);

		while (true)
			renderer.RenderFrame();
	}
}