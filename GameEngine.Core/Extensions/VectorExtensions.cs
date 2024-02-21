using GameEngine.Core.Extensions;
using System.Numerics;

namespace GameEngine.Extensions;

public static class VectorExtensions
{
	public static Vector3 ClampMagnitude(this Vector3 vec, float magnitude)
	{
		Vector3 clamped = vec;
		float length = vec.Length();
		if (length > magnitude)
			clamped = vec / length * magnitude;
		return clamped;
	}


	public static Vector2 ClampMagnitude(this Vector2 vec, float magnitude)
	{
		Vector2 clamped = vec;
		float length = vec.Length();
		if (length > magnitude)
			clamped = vec / length * magnitude;
		return clamped;
	}

	public static Vector3 RotateVectorByAxis(this Vector3 vec, Vector3 axis, float degree)
	{
		float radians = MathHelper.DegToRad(degree);
		Matrix4x4 rotationMatrix = Matrix4x4.CreateFromAxisAngle(axis, radians);
		return Vector3.Transform(vec, rotationMatrix);
	}
}