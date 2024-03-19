using GameEngine.Extensions;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace GameEngine.Core.Components;

public class Transform : INotifyPropertyChanged
{
	public static Vector3 GlobalRight { get; set; } = Vector3.UnitX;
	public static Vector3 GlobalUp { get; set; } = Vector3.UnitY;
	public static Vector3 GlobalFront { get; set; } = Vector3.UnitZ;

	private Vector3 _position, _rotation, _scale = Vector3.One;

	public Vector3 Position { get => _position; set { _position = value; OnPropertyChanged(); } }
	public Vector3 Rotation { get => _rotation; set { _rotation = value; CalculateLocalVectors(); OnPropertyChanged(); } }
	public Vector3 Scale { get => _scale; set { _scale = value; OnPropertyChanged(); } }

	public Vector3 LocalRight { get; private set; } = GlobalRight;
	public Vector3 LocalUp { get; private set; } = GlobalUp;
	public Vector3 LocalFront { get; private set; } = GlobalFront;

	public Transform() { }

	private void CalculateLocalVectors()
	{
		Matrix4x4 rotationMatrix =
			Matrix4x4.CreateRotationX(MathHelper.DegToRad(Rotation.X)) *
			Matrix4x4.CreateRotationY(MathHelper.DegToRad(Rotation.Y)) *
			Matrix4x4.CreateRotationZ(MathHelper.DegToRad(Rotation.Z));

		LocalRight = Vector3.Transform(GlobalRight, rotationMatrix);
		LocalUp = Vector3.Transform(GlobalUp, rotationMatrix);
		LocalFront = Vector3.Transform(GlobalFront, rotationMatrix);
	}

	public event PropertyChangedEventHandler? PropertyChanged;
	private void OnPropertyChanged([CallerMemberName] string caller = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
	}
}