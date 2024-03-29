using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;
using System.Numerics;

namespace BoxColliderSample;

internal class Ground : WorldObject
{
	public Ground(Vector2 size)
	{
		Tag = "Ground";

		Meshes.Add(new MeshData("Cube.obj", "Ground.mat"));
		Transform.Scale = new Vector3(size.X, 1, size.Y);
		BoxCollider = new BoxCollider(true, new Vector3(-size.X, -1, -size.Y), new Vector3(size.X, 1, size.Y));
	}
}