namespace GraphicsEngine.Components.Extensions;

public static class MatrixExtensions
{
	public static System.Numerics.Matrix4x4 ToNumerics(this OpenTK.Mathematics.Matrix4 matrix4)
		=> new System.Numerics.Matrix4x4(
			matrix4.Row0[0], matrix4.Row0[1], matrix4.Row0[2], matrix4.Row0[3],
			matrix4.Row1[0], matrix4.Row1[1], matrix4.Row1[2], matrix4.Row1[3],
			matrix4.Row2[0], matrix4.Row2[1], matrix4.Row2[2], matrix4.Row2[3],
			matrix4.Row3[0], matrix4.Row3[1], matrix4.Row3[2], matrix4.Row3[3]);

	public static OpenTK.Mathematics.Matrix4 ToOpenTK(this System.Numerics.Matrix4x4 matrix4)
		=> new OpenTK.Mathematics.Matrix4(
			matrix4.M11, matrix4.M12, matrix4.M13, matrix4.M14,
			matrix4.M21, matrix4.M22, matrix4.M23, matrix4.M24,
			matrix4.M31, matrix4.M32, matrix4.M33, matrix4.M34,
			matrix4.M41, matrix4.M42, matrix4.M43, matrix4.M44);
}