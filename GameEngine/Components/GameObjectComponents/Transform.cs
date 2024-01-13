using System.Numerics;

namespace GameEngine.Components.GameObjectComponents;

internal class Transform
{
	public readonly Action OnChanged;

	public static Vector3 GlobalRight { get; set; } = Vector3.UnitX;
	public static Vector3 GlobalUp { get; set; } = Vector3.UnitY;
	public static Vector3 GlobalFront { get; set; } = -Vector3.UnitZ;

	private Vector3 _position = Vector3.Zero, _rotation = Vector3.Zero, _scale = Vector3.Zero;
	public Vector3 Position { get => _position; set { _position = value; OnChanged(); } }
	public Vector3 Rotation { get => _rotation; set { _rotation = value; OnChanged(); } }
	public Vector3 Scale { get => _scale; set { _scale = value; OnChanged(); } }

	public Vector3 LocalRight { get; } = Vector3.UnitX;
	public Vector3 LocalUp { get; } = Vector3.UnitY;
	public Vector3 LocalFront { get; } = -Vector3.UnitZ;

	public Transform(Action onChanged)
	{
		OnChanged = onChanged;
	}
}