using OpenTK.Mathematics;

namespace GraphicsRenderer.Components.Shared.Data;

public class BoxData
{
	// Front
	public Vector3 TopLeftFront { get; set; }
	public Vector3 TopRightFront { get; set; }
	public Vector3 BottomRightFront { get; set; }
	public Vector3 BottomLeftFront { get; set; }

	// Back
	public Vector3 TopLeftBack { get; set; }
	public Vector3 TopRightBack { get; set; }
	public Vector3 BottomRightBack { get; set; }
	public Vector3 BottomLeftBack { get; set; }

	public BoxData(float right, float left, float top, float bottom, float front, float back)
	{
		TopLeftFront = new Vector3(left, top, front);
		TopRightFront = new Vector3(right, top, front);
		BottomRightFront = new Vector3(right, bottom, front);
		BottomLeftFront = new Vector3(left, bottom, front);

		TopLeftBack = new Vector3(left, top, back);
		TopRightBack = new Vector3(right, top, back);
		BottomRightBack = new Vector3(right, bottom, back);
		BottomLeftBack = new Vector3(left, bottom, back);
	}

	public float[] JoinAll()
	{
		float[] toReturn = new float[8 * 3];

		int i = 0;

		// 0
		toReturn[i++] = TopLeftFront.X;
		toReturn[i++] = TopLeftFront.Y;
		toReturn[i++] = TopLeftFront.Z;

		// 1
		toReturn[i++] = TopRightFront.X;
		toReturn[i++] = TopRightFront.Y;
		toReturn[i++] = TopRightFront.Z;

		// 2
		toReturn[i++] = BottomRightFront.X;
		toReturn[i++] = BottomRightFront.Y;
		toReturn[i++] = BottomRightFront.Z;

		// 3
		toReturn[i++] = BottomLeftFront.X;
		toReturn[i++] = BottomLeftFront.Y;
		toReturn[i++] = BottomLeftFront.Z;

		// 4
		toReturn[i++] = TopLeftBack.X;
		toReturn[i++] = TopLeftBack.Y;
		toReturn[i++] = TopLeftBack.Z;

		// 5
		toReturn[i++] = TopRightBack.X;
		toReturn[i++] = TopRightBack.Y;
		toReturn[i++] = TopRightBack.Z;

		// 6
		toReturn[i++] = BottomRightBack.X;
		toReturn[i++] = BottomRightBack.Y;
		toReturn[i++] = BottomRightBack.Z;

		// 7
		toReturn[i++] = BottomLeftBack.X;
		toReturn[i++] = BottomLeftBack.Y;
		toReturn[i++] = BottomLeftBack.Z;

		return toReturn;
	}
}