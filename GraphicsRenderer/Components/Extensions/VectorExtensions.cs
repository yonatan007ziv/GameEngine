namespace GraphicsRenderer.Components.Extensions;

internal static class VectorExtensions
{
	#region Vector2
	public static System.Numerics.Vector2 ToNumerics(this OpenTK.Mathematics.Vector2 vector) => new System.Numerics.Vector2(vector.X, vector.Y);
	public static OpenTK.Mathematics.Vector2 ToOpenTK(this System.Numerics.Vector2 vector) => new OpenTK.Mathematics.Vector2(vector.X, vector.Y);
	#endregion

	#region Vector3
	public static System.Numerics.Vector3 ToNumerics(this OpenTK.Mathematics.Vector3 vector) => new System.Numerics.Vector3(vector.X, vector.Y, vector.Z);
	public static OpenTK.Mathematics.Vector3 ToOpenTK(this System.Numerics.Vector3 vector) => new OpenTK.Mathematics.Vector3(vector.X, vector.Y, vector.Z);
	#endregion
}