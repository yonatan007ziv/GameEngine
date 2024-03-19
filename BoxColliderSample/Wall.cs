using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;
using System.Numerics;

namespace BoxColliderSample;

internal class Wall : WorldObject
{
	public Wall(Vector3 size)
	{
		Transform.Scale = size / 2;
		Meshes.Add(new MeshData("Cube.obj", "Wall.mat"));
		BoxCollider = new BoxCollider(true, -size / 2, size / 2);
	}
}