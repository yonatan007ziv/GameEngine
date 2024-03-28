using GameEngine.Extensions;
using System.Numerics;

namespace GameEngine.Core.Components.Fonts;

public class CharacterContour
{
    private readonly Vector2[] rawPoints; // Raw points representing both on curve and control points
    private readonly int[] controlPointIndexes; // All of the control points indexes in the rawPoints
    private readonly float parentGlyphWidth;
    private readonly float parentGlyphfHeight;
    private readonly int maxX = 0, minX = 0, maxY = 0, minY = 0;
    private Vector2[] transformedPoints; // Points after resolution (bezier curves) procedure
    private float rawCenterXCoordinate => (maxX + minX) / 2f;
    private float rawCenterYCoordinate => (maxY + minY) / 2f;

    // Backing fields
    private int _resolution;
    private float _fontSize;

    public IReadOnlyCollection<Vector2> Points { get; private set; }
    public int Resolution { get => _resolution; set { if (_resolution != value) { _resolution = value; RecalculatePoints(); RecalculateScale(); } } }
    public float FontSize { get => _fontSize; set { if (_fontSize != value) { _fontSize = value; RecalculateScale(); } } }
    public bool Clockwise { get; }

    public CharacterContour(Vector2[] rawPoints, int[] controlPointIndexes, float parentGlyphWidth, float parentGlyphfHeight, int resolution = 1, float fontSize = 1)
    {
        this.rawPoints = rawPoints;
        this.controlPointIndexes = controlPointIndexes;
        this.parentGlyphWidth = parentGlyphWidth;
        this.parentGlyphfHeight = parentGlyphfHeight;
        this._resolution = resolution;
        this._fontSize = fontSize;
        this.Clockwise = IsClockwise();

        transformedPoints = null!;
        Points = null!;
        Resolution = _resolution; // Calls the property set method

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
                // Resolution
                int segmentsPerControlPoint = _resolution + 1;
                for (int j = 0; j < segmentsPerControlPoint; j++)
                {
                    int t = (int)((j + 1) * (1f / segmentsPerControlPoint));

                    int x;
                    int y;

                    // Previous and next points also a control point
                    if (controlPointIndexes.Contains(i - 1) && controlPointIndexes.Contains(i + 1))
                    {
                        Vector2 averageBefore = (rawPoints[i - 1] + rawPoints[i]) / 2;
                        Vector2 averageAfter = (rawPoints[i] + rawPoints[(i + 1) % rawPoints.Length]) / 2;

                        x = (int)MathHelper.QBez(averageBefore.X, rawPoints[i].X, averageAfter.X, t);
                        y = (int)MathHelper.QBez(averageBefore.Y, rawPoints[i].Y, averageAfter.Y, t);
                    }
                    // Previous point also a control point
                    else if (controlPointIndexes.Contains(i - 1))
                    {
                        Vector2 averageBefore = (rawPoints[i - 1] + rawPoints[i]) / 2;

                        x = (int)MathHelper.QBez(averageBefore.X, rawPoints[i].X, rawPoints[(i + 1) % rawPoints.Length].X, t);
                        y = (int)MathHelper.QBez(averageBefore.Y, rawPoints[i].Y, rawPoints[(i + 1) % rawPoints.Length].X, t);
                    }
                    // Next point also a control point
                    else if (controlPointIndexes.Contains(i + 1))
                    {
                        Vector2 averageAfter = (rawPoints[i] + rawPoints[(i + 1) % rawPoints.Length]) / 2;

                        x = (int)MathHelper.QBez(rawPoints[(i == 0 ? rawPoints.Length : i) - 1].X, rawPoints[i].X, averageAfter.X, t);
                        y = (int)MathHelper.QBez(rawPoints[(i == 0 ? rawPoints.Length : i) - 1].Y, rawPoints[i].Y, averageAfter.Y, t);
                    }
                    // Control point surrounded by oncurve points
                    else
                    {
                        x = (int)MathHelper.QBez(rawPoints[(i == 0 ? rawPoints.Length : i) - 1].X, rawPoints[i].X, rawPoints[(i + 1) % rawPoints.Length].X, t);
                        y = (int)MathHelper.QBez(rawPoints[(i == 0 ? rawPoints.Length : i) - 1].Y, rawPoints[i].Y, rawPoints[(i + 1) % rawPoints.Length].Y, t);
                    }
                    points.Add(new Vector2(x, y));
                }
            }
            else
                points.Add(rawPoints[i]);
        }

        transformedPoints = points.ToArray();
    }

    private void RecalculateScale()
    {
        Vector2[] points = new Vector2[transformedPoints.Length];
        for (int i = 0; i < transformedPoints.Length; i++)
            points[i] = new Vector2((transformedPoints[i].X - rawCenterXCoordinate) / parentGlyphWidth, (transformedPoints[i].Y - rawCenterYCoordinate) / parentGlyphfHeight) * _fontSize;
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