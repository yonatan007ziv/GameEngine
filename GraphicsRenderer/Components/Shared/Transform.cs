using System.Numerics;

namespace GraphicsRenderer.Components.Shared;

internal class Transform
{
	public Vector3 Position { get; set; } = Vector3.Zero;
	public Vector3 Rotation { get; set; } = Vector3.Zero;
	public Vector3 Scale { get; set; } = Vector3.One;

	public Vector3 Right { get; set; } = Vector3.UnitX;
	public Vector3 Up { get; set; } = Vector3.UnitY;
	public Vector3 Front { get; set; } = Vector3.UnitZ;
}