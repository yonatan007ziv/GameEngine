using System.Numerics;

namespace GameEngine.Core.Components;

public class Transform
{
	public static Vector3 GlobalRight { get; set; } = Vector3.UnitX;
	public static Vector3 GlobalUp { get; set; } = Vector3.UnitY;
	public static Vector3 GlobalFront { get; set; } = -Vector3.UnitZ;

	public Vector3 Position { get; set; } = Vector3.Zero;
	public Vector3 Rotation { get; set; } = Vector3.Zero;
	public Vector3 Scale { get; set; } = Vector3.One;

	public Vector3 LocalRight { get; set; } = Vector3.UnitX;
	public Vector3 LocalUp { get; set; } = Vector3.UnitY;
	public Vector3 LocalFront { get; set; } = -Vector3.UnitZ;
}