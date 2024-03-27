using GameEngine.Core.Components.Fonts.TrueTypeFont.Tables;
using System.Numerics;

namespace GameEngine.Core.Components.Fonts;

public class Font
{
	private int _resolution;
	public int Resolution { get => _resolution; set { _resolution = value; foreach (CharacterGlyf glyf in CharacterMaps.Values) glyf.Resolution = value; } }

	public string FontName { get; }
	public string FontFamily { get; }
	public string FontSubFamily { get; }
	public string Style { get; }
	public string Version { get; }

	public IReadOnlyDictionary<char, CharacterGlyf> CharacterMaps { get; }

	// TTF font constructor
	internal Font(TTFHead head, TTFName name, TTFLoca loca, TTFGlyf glyf, TTFCmap cmap, TTFHhea hhea, TTFVhea vhea, TTFHmtx hmtx, TTFVmtx vmtx, TTFMaxp maxp)
	{
		FontName = name.Name[4];
		FontFamily = name.Name[1];
		FontSubFamily = name.Name[2];
		Version = head.FontRevision.ToString();

		List<KeyValuePair<char, CharacterGlyf>> characterMaps = new List<KeyValuePair<char, CharacterGlyf>>();

		CharacterGlyf[] glyphs = new CharacterGlyf[maxp.NumGlyphs];

		// Glyphs
		for (int i = 0; i < maxp.NumGlyphs; i++)
		{
			int maxX = 0, minX = 0, maxY = 0, minY = 0;

			// Compound glyph (not supported)
			if (glyf.Glyphs[i].NumberOfContours < 0)
				continue;

			// Contours
			CharacterContour[] characterContours = new CharacterContour[glyf.Glyphs[i].NumberOfContours];
			for (int j = 0; j < glyf.Glyphs[i].NumberOfContours; j++)
			{
				List<Vector2> contourPoints = new List<Vector2>();
				List<int> controlPointIndexes = new List<int>();

				if (i == 36)
				{

				}

				// Iterate through all of the points in the current contour
				int k = j == 0 ? 0 : (glyf.Glyphs[i].EndPtsOfContours[j - 1] + 1);
				for (; k < glyf.Glyphs[i].EndPtsOfContours[j] + 1; k++)
				{
					// Is not OnCurve 
					if ((glyf.Glyphs[i].FlagsSimple[k] & (int)TTFGlyf.SIMPLE_FLAGS.ON_CURVE) == 0)
						controlPointIndexes.Add(k);

					int x = glyf.Glyphs[i].XCoordinates[k], y = glyf.Glyphs[i].YCoordinates[k];

					maxX = Math.Max(maxX, x);
					minX = Math.Min(minX, x);
					maxY = Math.Max(maxY, y);
					minY = Math.Min(minY, y);

					contourPoints.Add(new Vector2(x, y));
				}

				characterContours[j] = new CharacterContour(contourPoints.ToArray(), controlPointIndexes.ToArray(), 2);
			}

			glyphs[i] = new CharacterGlyf(characterContours, Math.Abs(maxX - minX), Math.Abs(maxY - minY));
		}

		#region temp
		// Scilab source code
		int offsetX = 0;

		int[] indexes = new int[26];
		for (int i = 0; i < 26; i++)
			indexes[i] = i + 36;

		List<string> sourceCodeLines = ["clear;", "clc;"];

		for (int j = 0; j < indexes.Length; j++)
		{
			CharacterGlyf currentGlyph = glyphs[indexes[j]];

			foreach (CharacterContour contour in currentGlyph.CharacterContours)
			{
				sourceCodeLines.Add("x = [");
				foreach (Vector2 point in contour.Points)
					sourceCodeLines.Add($"{(int)point.X}, {(int)point.Y};");
				sourceCodeLines.Add("];");


				for (int i = 0; i < contour.Points.Count; i++)
					sourceCodeLines.Add($"plot([x({i + 1}, 1), x({i + 2}, 1)], [x({i + 1}, 2), x({i + 2}, 2)]);");
				// Wrap
				sourceCodeLines.Add($"plot([x({contour.Points.Count}, 1), x({1}, 1)], [x({contour.Points.Count}, 2), x({1}, 2)]);");

				sourceCodeLines.Add("scatter(x(:, 1), x(:, 2));");
			}
		}

		// Add to offset
		offsetX += 1750;

		// Wrap up plotter
		sourceCodeLines.Add("xlabel('X');");
		sourceCodeLines.Add("ylabel('Y');");
		sourceCodeLines.Add("xtitle('Scatter Plot', 'X', 'Y');");

		File.WriteAllLines(@"D:\Scilab.txt", sourceCodeLines);

		#endregion
		CharacterMaps = new Dictionary<char, CharacterGlyf>(characterMaps);
	}
}