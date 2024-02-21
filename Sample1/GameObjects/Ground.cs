using GameEngine.Components.Objects;
using GameEngine.Core.Components;
using System.Numerics;

namespace Sample1.GameObjects;

internal class Ground : WorldObject
{
	public Ground(Vector2 size)
	{
		Meshes.Add(new MeshData("UIPlane.obj", "Ground.mat"));
		Transform.Scale = new Vector3(size.X, 10, size.Y);
		Transform.Rotation += new Vector3(90, 0, 0);
	}
}