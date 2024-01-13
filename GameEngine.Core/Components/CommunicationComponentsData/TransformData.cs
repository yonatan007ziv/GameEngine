using System.Numerics;

namespace GameEngine.Core.Components.CommunicationComponentsData;

public struct TransformData
{
	public Vector3 Position { get; set; } = Vector3.Zero;
	public Vector3 Rotation { get; set; } = Vector3.Zero;
	public Vector3 Scale { get; set; } = Vector3.One;

	public Vector3 LocalRight { get; set; } = Vector3.UnitX;
	public Vector3 LocalUp { get; set; } = Vector3.UnitY;
	public Vector3 LocalFront { get; set; } = -Vector3.UnitZ;

	public TransformData(Vector3 position, Vector3 rotation, Vector3 scale)
	{
		Position = position;
		Rotation = rotation;
		Scale = scale;
	}
}