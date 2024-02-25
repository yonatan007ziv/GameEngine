using System.Numerics;

namespace GameEngine.Core.Components.Font;

internal class CharacterContour
{
	public Vector2[] Points { get; }

	public CharacterContour(Vector2[] points)
	{
		Points = points;
	}

	public bool IsClockwise()
	{
		// Implement shoelace theorem to check for clockwise orientation
		double shoelaceSum = 0;
		for (int i = 0; i < Points.Length; i++)
		{
			Vector2 p1 = Points[i];
			Vector2 p2 = Points[(i + 1) % Points.Length]; // Wrap around for last point

			shoelaceSum += (p1.X * p2.Y - p2.X * p1.Y);
		}

		return shoelaceSum > 0; // Clockwise if positive, counter-clockwise otherwise
	}
}