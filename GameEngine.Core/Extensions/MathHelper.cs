namespace GameEngine.Extensions;

public static partial class MathHelper
{
	public static float RadToDeg(float rad) => rad * 180 / MathF.PI;
	public static float DegToRad(float deg) => deg * MathF.PI / 180;

	// Linear interpolation - a is the start value, b the end value and t is the 'time'
	public static float Lerp(float a, float b, float t)
	{
		return (1 - t) * a + t * b;
	}

	// Quadratic bezier interpolation - a and c are on the curve while b is the control point and t is the 'time'
	public static float QBez(float a, float b, float c, float t)
	{
		// Can expand this to the strict formula, but idc enough the stack pointer can kiss ass
		return Lerp(Lerp(a, b, t), Lerp(b, c, t), t);
	}

}