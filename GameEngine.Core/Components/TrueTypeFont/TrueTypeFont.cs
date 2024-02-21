using GameEngine.Core.Components.TrueTypeFont.Tables;

namespace GameEngine.Core.Components.TrueTypeFont;

public class TrueTypeFont
{
	public string Name { get; }
	public string FamilyName { get; }
	public string Style { get; }
	public string version { get; }

	private readonly Dictionary<char, Glyf> characterMaps = new Dictionary<char, Glyf>();
}