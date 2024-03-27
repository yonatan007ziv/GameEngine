using GameEngine.Core.Components.Fonts.TrueTypeFont.Tables;

namespace GameEngine.Core.Components.Fonts;

public class Font
{
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

		for (int i = 0; i < maxp.NumGlyphs; i++)
		{
			//char characterCode = cmap.GetMapping(i); // Get character code from cmap
			//
			//int glyphIndex = loca.GetGlyphIndex(i); // Get glyph index from loca
			//if (glyphIndex == -1)
			//	continue; // Skip if glyph index is invalid
			//
			//int width = hmtx.GetAdvanceWidth(glyphIndex); // Get width from hmtx
			//int height = vmtx.GetAdvanceHeight(glyphIndex); // Get height from vmtx
			//
			//CharacterContour[] contours = glyf.GetContours(glyphIndex); // Get contours from glyf
			//
			//CharacterGlyf characterGlyf = new CharacterGlyf(contours, width, height);
			//characterMaps.Add(KeyValuePair.Create(characterCode, characterGlyf));
		}

		CharacterMaps = new Dictionary<char, CharacterGlyf>(characterMaps);
	}
}