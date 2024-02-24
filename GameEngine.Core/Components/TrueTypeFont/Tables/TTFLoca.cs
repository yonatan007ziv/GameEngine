namespace GameEngine.Core.Components.TrueTypeFont.Tables;

internal class TTFLoca
{
	public uint[] Offsets { get; }

    public TTFLoca(uint[] offsets)
    {
        Offsets = offsets;
    }
}