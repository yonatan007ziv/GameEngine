using GameEngine.Components.Objects;
using GameEngine.Core.Components;

namespace Sample1.GameObjects;

internal class Trex : WorldObject
{
	public Trex()
	{
		Meshes.Add(new MeshData("Trex.obj", "Hamama.mat"));
	}
}