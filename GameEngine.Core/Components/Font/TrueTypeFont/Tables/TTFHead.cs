namespace GameEngine.Core.Components.Font.TrueTypeFont.Tables;

internal class TTFHead
{
	public float Version { get; }
	public float FontRevision { get; }
	public uint CheckSumAdjustment { get; }
	public uint MagicNumber { get; }
	public ushort Flags { get; }
	public ushort UnitsPerEm { get; }
	public float Created { get; }
	public float Modified { get; }
	public short XMin { get; }
	public short YMin { get; }
	public short XMax { get; }
	public short YMax { get; }
	public ushort MacStyle { get; }
	public ushort LowestRecPPEM { get; }
	public short FontDirectionHint { get; }
	public short IndexToLocFormat { get; }
	public short GlyphDataFormat { get; }

	public TTFHead(float version, float fontRevision, uint checkSumAdjustment, uint magicNumber, ushort flags, ushort unitsPerEm, float created, float modified, short xMin, short yMin, short xMax, short yMax, ushort macStyle, ushort lowestRecPPEM, short fontDirectionHint, short indexToLocFormat, short glyphDataFormat)
	{
		Version = version;
		FontRevision = fontRevision;
		CheckSumAdjustment = checkSumAdjustment;
		MagicNumber = magicNumber;
		Flags = flags;
		UnitsPerEm = unitsPerEm;
		Created = created;
		Modified = modified;
		XMin = xMin;
		YMin = yMin;
		XMax = xMax;
		YMax = yMax;
		MacStyle = macStyle;
		LowestRecPPEM = lowestRecPPEM;
		FontDirectionHint = fontDirectionHint;
		IndexToLocFormat = indexToLocFormat;
		GlyphDataFormat = glyphDataFormat;
	}
}