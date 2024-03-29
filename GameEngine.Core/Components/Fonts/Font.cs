using GameEngine.Core.Components.Fonts.TrueTypeFont.Tables;
using System.Numerics;
using static GameEngine.Core.Components.Fonts.TrueTypeFont.Tables.TTFCmap;

namespace GameEngine.Core.Components.Fonts;

public class Font
{
	private int _resolution;
	private float _fontSize;

	public int Resolution { get => _resolution; set { if (_resolution != value) { _resolution = value; foreach (CharacterGlyf glyf in CharacterMaps.Values) if (glyf is not null) glyf.Resolution = value; } } }
	public float FontSize { get => _fontSize; set { if (_fontSize != value) { _fontSize = value; foreach (CharacterGlyf glyf in CharacterMaps.Values) if (glyf is not null) glyf.FontSize = value; } } }

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

		CharacterGlyf[] glyphs = new CharacterGlyf[maxp.NumGlyphs];

		// Glyphs
		for (int i = 0; i < maxp.NumGlyphs; i++)
		{
			// Compound glyph (not supported)
			if (glyf.Glyphs[i].NumberOfContours < 0)
				continue;

			float glyphWidth = Math.Abs(glyf.Glyphs[i].XMax - glyf.Glyphs[i].XMin);
			float glyphHeight = Math.Abs(glyf.Glyphs[i].YMax - glyf.Glyphs[i].YMin);

			float glyphCenterX = (glyf.Glyphs[i].XMax + glyf.Glyphs[i].XMin) / 2f;
			float glyphCenterY = (glyf.Glyphs[i].YMax + glyf.Glyphs[i].YMin) / 2f;

			// Contours
			CharacterContour[] characterContours = new CharacterContour[glyf.Glyphs[i].NumberOfContours];
			for (int j = 0; j < glyf.Glyphs[i].NumberOfContours; j++)
			{
				List<Vector2> contourPoints = new List<Vector2>();
				List<int> controlPointIndexes = new List<int>();

				// Iterate through all of the points in the current contour
				int startingK = j == 0 ? 0 : (glyf.Glyphs[i].EndPtsOfContours[j - 1] + 1);
				int k = startingK;
				for (; k < glyf.Glyphs[i].EndPtsOfContours[j] + 1; k++)
				{
					// Not on curve 
					if ((glyf.Glyphs[i].FlagsSimple[k] & (int)TTFGlyf.SIMPLE_FLAGS.ON_CURVE) == 0)
						controlPointIndexes.Add(k - startingK);

					int x = glyf.Glyphs[i].XCoordinates[k], y = glyf.Glyphs[i].YCoordinates[k];
					contourPoints.Add(new Vector2(x, y));
				}

				if (j == 37)
				{

				}
				characterContours[j] = new CharacterContour(contourPoints.ToArray(), controlPointIndexes.ToArray(), glyphWidth, glyphHeight, glyphCenterX, glyphCenterY);
			}

			glyphs[i] = new CharacterGlyf(characterContours);
		}

		CharacterMaps = GetTTFCmapMappings(glyphs, cmap);
		FontSize = 1;
	}

	private IReadOnlyDictionary<char, CharacterGlyf> GetTTFCmapMappings(CharacterGlyf[] glyphs, TTFCmap cmap)
	{
		// Prefer subtable format 4
		SubtableFormat4? subtableFormat4 = null;
		foreach (Subtable table in cmap.Subtables)
			if (table is SubtableFormat4 format4)
				subtableFormat4 = format4;

		Dictionary<char, CharacterGlyf> mappingDictionary = new Dictionary<char, CharacterGlyf>();

		if (subtableFormat4 is not null)
		{
			int idDeltaIndex = 0;
			for (int codeIndex = 0; codeIndex < subtableFormat4.StartCode.Length; codeIndex++)
			{
				for (int currentCode = subtableFormat4.StartCode[codeIndex]; currentCode <= subtableFormat4.EndCode[codeIndex]; currentCode++)
					if ((currentCode + subtableFormat4.IdDelta[idDeltaIndex]) % 0xFFFF - 1 < glyphs.Length)
						mappingDictionary.Add((char)currentCode, glyphs[(currentCode + subtableFormat4.IdDelta[idDeltaIndex]) % 0xFFFF - 1]);

				idDeltaIndex++;
			}
			return mappingDictionary;
		}
		else
		{
			// Other table format
		}

		return new Dictionary<char, CharacterGlyf>();
	}
}