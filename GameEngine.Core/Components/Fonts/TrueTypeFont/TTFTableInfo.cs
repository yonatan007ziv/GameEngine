namespace GameEngine.Core.Components.Fonts.TrueTypeFont;

internal struct TTFTableInfo
{
    public string tag;
    public uint checksum;
    public uint offset;
    public uint length;

    public TTFTableInfo(string tag, uint checksum, uint offset, uint length)
    {
        this.tag = tag;
        this.checksum = checksum;
        this.offset = offset;
        this.length = length;
    }
}