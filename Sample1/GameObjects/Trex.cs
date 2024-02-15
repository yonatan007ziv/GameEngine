using GameEngine.Components;
using GameEngine.Core.Components;

namespace Sample1.GameObjects;

internal class Trex : GameObject
{
	public Trex()
	{
		Meshes.Add(new MeshData("Trex.obj", "Hamama.mat"));
	}
}