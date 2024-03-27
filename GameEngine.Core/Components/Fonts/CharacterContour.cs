using GameEngine.Extensions;
using System.Numerics;

namespace GameEngine.Core.Components.Fonts;

public class CharacterContour
{
	// On curve and control points
	private readonly Vector2[] rawPoints;
	private readonly int[] controlPointIndexes;

	public IReadOnlyCollection<Vector2> Points { get; private set; }

	private int resolution;
	public int Resolution { get => resolution; set { resolution = value; RecalculatePoints(); } }

	public CharacterContour(Vector2[] rawPoints, int[] controlPointIndexes, int resolution)
	{
		this.rawPoints = rawPoints;
		this.controlPointIndexes = controlPointIndexes;

		Resolution = resolution;

		Points = null!;
		RecalculatePoints();
	}

	private void RecalculatePoints()
	{
		List<Vector2> points = new List<Vector2>();

		for(int i = 0; i < rawPoints.Length; i++)
		{
			// Control point
			if (controlPointIndexes.Contains(i))
			{
				/*
				float t = k * (1f / (Resolution + 1));

				int x;
				int y;

				// Previous point also a control point
				if (controlPointIndexes.Contains(i - 1) && controlPointIndexes.Contains(i + 1))
				{
					Vector2 averageBefore = (rawPoints[i - 1] + rawPoints[i]) / 2;
					Vector2 averageAfter = (rawPoints[i] + rawPoints[i + 1]) / 2;


				}
				else if (controlPointIndexes.Contains(i - 1))
				{
					Vector2 averageBefore = (rawPoints[i - 1] + rawPoints[i]) / 2;

					x = MathHelper.QBez();
					y = MathHelper.QBez();
				}
				// Next point also a control point
				else if (controlPointIndexes.Contains(i + 1))
				{
					Vector2 averageAfter = (rawPoints[i] + rawPoints[i + 1]) / 2;

					x = MathHelper.QBez();
					y = MathHelper.QBez();
				}
				// Control point surrounded by oncurve points
				else
				{
					x = MathHelper.QBez();
					y = MathHelper.QBez();
				}

				points.Add(new Vector2(x, y));
				*/
			}
			else
			{
				points.Add(rawPoints[i]);
			}
		}
	}

	public bool IsClockwise()
	{
		// Implement shoelace theorem to check for clockwise orientation
		double shoelaceSum = 0;
		for (int i = 0; i < Points.Count; i++)
		{
			Vector2 p1 = Points.ElementAt(i);
			Vector2 p2 = Points.ElementAt((i + 1) % Points.Count); // Wrap around for last point

			shoelaceSum += p1.X * p2.Y - p2.X * p1.Y;
		}

		return shoelaceSum > 0; // Clockwise if positive, counter-clockwise otherwise
	}
}