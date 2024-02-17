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
}