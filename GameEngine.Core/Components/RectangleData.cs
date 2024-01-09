using System.Numerics;

namespace GameEngine.Core.Components;

public struct RectangleData
{
	public Vector3 TopRight { get; set; }
	public Vector3 BottomLeft { get; set; }

	public RectangleData(float right, float left, float top, float bottom, float z)
	{
		TopRight = new Vector3(right, top, z);
		BottomLeft = new Vector3(left, bottom, z);
	}
}