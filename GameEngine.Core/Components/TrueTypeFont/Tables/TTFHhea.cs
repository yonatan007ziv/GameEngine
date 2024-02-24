namespace GameEngine.Core.Components.TrueTypeFont.Tables;

internal class TTFHhea
{
	public float Version { get; }
	public short Ascent { get; }
	public short Descent { get; }
	public short LineGap { get; }
	public ushort AdvanceWidthMax { get; }
	public short MinLeftSideBearing { get; }
	public short MinRightSideBearing { get; }
	public short XMaxExtent { get; }
	public short CaretSlopeRise { get; }
	public short CaretSlopeRun { get; }
	public short CaretOffset { get; }
	public short Reserved1 { get; }
	public short Reserved2 { get; }
	public short Reserved3 { get; }
	public short Reserved4 { get; }
	public short MetricDataFormat { get; }
	public ushort NumOfLongHorMetrics { get; }

	public TTFHhea(float version, short ascent, short descent, short lineGap, ushort advanceWidthMax, short minLeftSideBearing, short minRightSideBearing, short xMaxExtent, short caretSlopeRise, short caretSlopeRun, short caretOffset, short reserved1, short reserved2, short reserved3, short reserved4, short metricDataFormat, ushort numOfLongHorMetrics)
	{
		Version = version;
		Ascent = ascent;
		Descent = descent;
		LineGap = lineGap;
		AdvanceWidthMax = advanceWidthMax;
		MinLeftSideBearing = minLeftSideBearing;
		MinRightSideBearing = minRightSideBearing;
		XMaxExtent = xMaxExtent;
		CaretSlopeRise = caretSlopeRise;
		CaretSlopeRun = caretSlopeRun;
		CaretOffset = caretOffset;
		Reserved1 = reserved1;
		Reserved2 = reserved2;
		Reserved3 = reserved3;
		Reserved4 = reserved4;
		MetricDataFormat = metricDataFormat;
		NumOfLongHorMetrics = numOfLongHorMetrics;
	}
}