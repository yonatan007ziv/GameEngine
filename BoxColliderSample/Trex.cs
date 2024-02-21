using GameEngine.Components.Objects;
using GameEngine.Core.Components;

namespace BoxColliderSample;

internal class Trex : WorldObject
{
	public Trex()
	{
		Meshes.Add(new MeshData("Trex.obj", "Trex.mat"));
	}
}