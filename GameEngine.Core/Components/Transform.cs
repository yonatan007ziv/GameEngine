using GameEngine.Core.Extensions;
using System.Numerics;

namespace GameEngine.Core.Components;

public class Transform
{
	public static Vector3 GlobalRight { get; set; } = Vector3.UnitX;
	public static Vector3 GlobalUp { get; set; } = Vector3.UnitY;
	public static Vector3 GlobalFront { get; set; } = -Vector3.UnitZ;

	public bool TransformDirty { get; set; }

	private Vector3 _position, _rotation, _scale;
	public Vector3 Position { get => _position; set { _position = value; TransformDirty = true; } }
	public Vector3 Rotation { get => _rotation; set { _rotation = value; TransformDirty = true; CalculateLocalVectors(); } }
	public Vector3 Scale { get => _scale; set { _scale = value; TransformDirty = true; } }

	public Vector3 LocalRight { get; private set; }
	public Vector3 LocalUp { get; private set; }
	public Vector3 LocalFront { get; private set; }

	private void CalculateLocalVectors()
	{
		LocalFront = Vector3.Normalize(
			new Vector3(
			MathF.Cos(MathHelper.DegToRad(Rotation.X)) * MathF.Cos(MathHelper.DegToRad(Rotation.Y)),
			MathF.Sin(MathHelper.DegToRad(Rotation.X)),
			MathF.Cos(MathHelper.DegToRad(Rotation.X)) * MathF.Sin(MathHelper.DegToRad(Rotation.Y))
			));
		LocalRight = Vector3.Transform(Vector3.Normalize(Vector3.Cross(LocalFront, Vector3.UnitY)), Matrix4x4.CreateRotationX(MathHelper.DegToRad(Rotation.Z)));
		LocalUp = Vector3.Normalize(Vector3.Cross(LocalRight, LocalFront));
	}
}