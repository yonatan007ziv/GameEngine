using GameEngine.Core.Extensions;
using System.Numerics;

namespace GameEngine.Extensions;

public static class VectorExtensions
{
	public static Vector3 ClampYZTo360Degrees(this Vector3 vec)
	{
		// Ensure rotation is within 0-360
		float x = (vec.X % 360 + 360) % 360;
		float y = (vec.Y % 360 + 360) % 360;
		float z = (vec.Z % 360 + 360) % 360;

		// Clamp rotation between -180 and 180
		x = (x > 180) ? x - 360 : x;
		y = (y > 180) ? y - 360 : y;
		z = (z > 180) ? z - 360 : z;

		return new Vector3(x, y, z);


		//float y = (vec.Y % 360 + 360) % 360;
		//float z = (vec.Z % 360 + 360) % 360;
		//return new Vector3(vec.X, y, z);
	}

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