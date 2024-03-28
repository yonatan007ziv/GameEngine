namespace GameEngine.Core.Components.Fonts.TrueTypeFont.Tables;

internal class TTFLoca
{
    public uint[] Offsets { get; }

    public TTFLoca(uint[] offsets)
    {
        Offsets = offsets;
    }
}