using GameEngine.Core.Components.TrueTypeFont.Tables;

namespace GameEngine.Core.Components.TrueTypeFont;

public class TrueTypeFont
{
	public string Name { get; }
	public string FamilyName { get; }
	public string Style { get; }
	public string version { get; }

	private readonly Dictionary<char, TTFGlyf> characterMaps = new Dictionary<char, TTFGlyf>();

	internal TrueTypeFont(TTFHead head, TTFName name, TTFLoca loca, TTFGlyf glyf, TTFCmap cmap, TTFHhea hhea, TTFVhea vhea, TTFHmtx hmtx, TTFVmtx vmtx, TTFMaxp maxp)
	{

	}
}