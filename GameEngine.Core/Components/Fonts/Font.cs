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

		for (int i = 0; i < maxp.NumGlyphs; i++)
		{
			// CMAP and GLYF
		}

		KeyValuePair<char, CharacterGlyf>[] characterMaps = null;
		CharacterMaps = new Dictionary<char, CharacterGlyf>(characterMaps);
	}
}