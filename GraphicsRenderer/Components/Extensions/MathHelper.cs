namespace GraphicsRenderer.Components.Extensions;

public static partial class MathHelper
{
	public static float RadToDeg(float rad) => rad * 180 / MathF.PI;
	public static float DegToRad(float deg) => deg * MathF.PI / 180;
}