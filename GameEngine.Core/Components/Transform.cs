using GameEngine.Core.Components.CommunicationComponentsData;
using GameEngine.Core.Extensions;
using System.Numerics;

namespace GameEngine.Core.Components;

public class Transform
{
	public static Vector3 GlobalRight { get; set; } = Vector3.UnitX;
	public static Vector3 GlobalUp { get; set; } = Vector3.UnitY;
	public static Vector3 GlobalFront { get; set; } = -Vector3.UnitZ;

	private Vector3 _position, _rotation, _scale;
	public Vector3 Position { get => _position; set { _position = value; } }
	public Vector3 Rotation { get => _rotation; set { _rotation = value; CalculateLocalVectors(); } }
	public Vector3 Scale { get => _scale; set { _scale = value; } }

	public Vector3 LocalRight { get; private set; }
	public Vector3 LocalUp { get; private set; }
	public Vector3 LocalFront { get; private set; }

	public Transform() { }
	public Transform(TransformData transform) { Copy(transform); }

	public void Copy(TransformData transform)
	{
		Position = transform.Position;
		Rotation = transform.Rotation;
		Scale = transform.Scale;
	}

	private void CalculateLocalVectors()
	{
		LocalRight = Vector3.Transform(Vector3.Normalize(Vector3.Cross(LocalFront, Vector3.UnitY)), Matrix4x4.CreateRotationX(MathHelper.DegToRad(Rotation.Z)));
		LocalUp = Vector3.Normalize(Vector3.Cross(LocalRight, LocalFront));
		LocalFront = Vector3.Normalize(
			new Vector3(
			MathF.Cos(MathHelper.DegToRad(Rotation.X)) * MathF.Cos(MathHelper.DegToRad(Rotation.Y)),
			MathF.Sin(MathHelper.DegToRad(Rotation.X)),
			MathF.Cos(MathHelper.DegToRad(Rotation.X)) * MathF.Sin(MathHelper.DegToRad(Rotation.Y))
			));
	}
}