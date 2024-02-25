namespace GameEngine.Core.Components.Font.TrueTypeFont.Tables;

internal class TTFLoca
{
	public uint[] Offsets { get; }

	public TTFLoca(uint[] offsets)
	{
		Offsets = offsets;
	}
}