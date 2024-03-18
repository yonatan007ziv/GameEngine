using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;

namespace BoxColliderSample;

internal class Trex : WorldObject
{
	public Trex()
	{
		Meshes.Add(new MeshData("Trex.obj", "Trex.mat"));
	}
}