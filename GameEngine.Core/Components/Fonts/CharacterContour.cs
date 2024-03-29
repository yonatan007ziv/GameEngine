using GameEngine.Extensions;
using System.Numerics;

namespace GameEngine.Core.Components.Fonts;

public class CharacterContour
{
	private readonly Vector2[] rawPoints; // Raw points representing both on curve and control points
	private readonly int[] controlPointIndexes; // All of the control points indexes in the rawPoints
	private readonly float parentGlyphWidth;
	private readonly float parentGlyphHeight;
	private readonly float parentCenterXCoordinate;
	private readonly float parentCenterYCoordinate;
	private readonly int maxX = 0, minX = 0, maxY = 0, minY = 0;
	private Vector2[] transformedCenteredPoints; // Points after resolution (bezier curves) procedure

	// Backing fields
	private int _resolution;
	private float _fontSize;

	public IReadOnlyCollection<Vector2> Points { get; private set; }
	public int Resolution { get => _resolution; set { if (_resolution != value) { _resolution = value; RecalculatePoints(); RecalculateScale(); } } }
	public float FontSize { get => _fontSize; set { if (_fontSize != value) { _fontSize = value; RecalculateScale(); } } }
	public bool Clockwise { get; }

	public CharacterContour(Vector2[] rawPoints, int[] controlPointIndexes, float parentGlyphWidth, float parentGlyphHeight, float parentCenterXCoordinate, float parentCenterYCoordinate, int resolution = 5, float fontSize = 1)
	{
		this.rawPoints = rawPoints;
		this.controlPointIndexes = controlPointIndexes;
		this.parentGlyphWidth = parentGlyphWidth;
		this.parentGlyphHeight = parentGlyphHeight;
		this.parentCenterXCoordinate = parentCenterXCoordinate;
		this.parentCenterYCoordinate = parentCenterYCoordinate;
		this._resolution = resolution;
		this._fontSize = fontSize;
		this.Clockwise = IsClockwise();

		transformedCenteredPoints = null!;
		Points = null!;

		// Raw contour height and width
		maxX = (int)rawPoints[0].X;
		minX = (int)rawPoints[0].X;
		maxY = (int)rawPoints[0].Y;
		minY = (int)rawPoints[0].Y;

		foreach (Vector2 vec in rawPoints)
		{
			int x = (int)vec.X;
			int y = (int)vec.Y;

			maxX = Math.Max(maxX, x);
			minX = Math.Min(minX, x);
			maxY = Math.Max(maxY, y);
			minY = Math.Min(minY, y);
		}

		RecalculatePoints();
		RecalculateScale();
	}

	private void RecalculatePoints()
	{
		List<Vector2> points = new List<Vector2>();

		for (int i = 0; i < rawPoints.Length; i++)
		{
			// Control point
			if (controlPointIndexes.Contains(i))
			{
				// Resolution for bezier curves
				for (int j = 0; j < _resolution; j++)
				{
					// Interpolation value t
					float t = (j + 1) * (1f / (_resolution + 1));

					int x;
					int y;

					// Both include wrapping forward and backwards
					int previousPointIndex = (i - 1 + rawPoints.Length) % rawPoints.Length;
					int nextPointIndex = (i + 1) % rawPoints.Length;

					Vector2 before = rawPoints[previousPointIndex];
					Vector2 currentControlPoint = rawPoints[i];
					Vector2 after = rawPoints[nextPointIndex];

					Vector2 averageBefore = (rawPoints[previousPointIndex] + rawPoints[i]) / 2;
					Vector2 averageAfter = (rawPoints[i] + rawPoints[nextPointIndex]) / 2;

					// Previous and next points also a control point
					if (controlPointIndexes.Contains(previousPointIndex) && controlPointIndexes.Contains(nextPointIndex))
					{
						x = (int)MathHelper.QBez(averageBefore.X, currentControlPoint.X, averageAfter.X, t);
						y = (int)MathHelper.QBez(averageBefore.Y, currentControlPoint.Y, averageAfter.Y, t);
					}
					// Previous point also a control point
					else if (controlPointIndexes.Contains(i - 1))
					{
						x = (int)MathHelper.QBez(averageBefore.X, currentControlPoint.X, after.X, t);
						y = (int)MathHelper.QBez(averageBefore.Y, currentControlPoint.Y, after.Y, t);
					}
					// Next point also a control point
					else if (controlPointIndexes.Contains(i + 1))
					{
						x = (int)MathHelper.QBez(before.X, currentControlPoint.X, averageAfter.X, t);
						y = (int)MathHelper.QBez(before.Y, currentControlPoint.Y, averageAfter.Y, t);
					}
					// Control point surrounded by oncurve points
					else
					{
						x = (int)MathHelper.QBez(before.X, currentControlPoint.X, after.X, t);
						y = (int)MathHelper.QBez(before.Y, currentControlPoint.Y, after.Y, t);
					}
					points.Add(new Vector2(x - parentCenterXCoordinate, y - parentCenterYCoordinate));
				}
			}
			else
				points.Add(rawPoints[i] - new Vector2(parentCenterXCoordinate, parentCenterYCoordinate));
		}

		transformedCenteredPoints = points.ToArray();
	}

	private void RecalculateScale()
	{
		float length = (float)Math.Sqrt(Math.Pow(parentGlyphWidth, 2) + Math.Pow(parentGlyphHeight, 2));

		Vector2[] points = new Vector2[transformedCenteredPoints.Length];
		for (int i = 0; i < transformedCenteredPoints.Length; i++)
			points[i] = new Vector2(transformedCenteredPoints[i].X, transformedCenteredPoints[i].Y) / length * _fontSize;

		Points = points;
	}

	private bool IsClockwise()
	{
		// Implement shoelace theorem to check for clockwise orientation
		double shoelaceSum = 0;
		for (int i = 0; i < rawPoints.Length; i++)
		{
			Vector2 p1 = rawPoints[i];
			Vector2 p2 = rawPoints[(i + 1) % rawPoints.Length]; // Wrap around for last point

			shoelaceSum += p1.X * p2.Y - p2.X * p1.Y;
		}

		return shoelaceSum > 0; // Clockwise if positive, counter-clockwise otherwise
	}
}