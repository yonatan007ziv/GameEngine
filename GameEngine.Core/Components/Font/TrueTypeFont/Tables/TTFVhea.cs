namespace GameEngine.Core.Components.Font.TrueTypeFont.Tables;

internal class TTFVhea
{
	public float Version { get; }
	public short VertTypoAscender { get; }
	public short VertTypoDescender { get; }
	public short VertTypoLineGap { get; }
	public short AdvanceHeightMax { get; }
	public short MinTopSideBearing { get; }
	public short MinBottomSideBearing { get; }
	public short YMaxExtent { get; }
	public short CaretSlopeRise { get; }
	public short CaretSlopeRun { get; }
	public short CaretOffset { get; }
	public short Reserved1 { get; }
	public short Reserved2 { get; }
	public short Reserved3 { get; }
	public short Reserved4 { get; }
	public short MetricDataFormat { get; }
	public ushort NumOfLongVerMetrics { get; }

	public TTFVhea(float version, short vertTypoAscender, short vertTypoDescender, short vertTypoLineGap, short advanceHeightMax, short minTopSideBearing, short minBottomSideBearing, short yMaxExtent, short caretSlopeRise, short caretSlopeRun, short caretOffset, short reserved1, short reserved2, short reserved3, short reserved4, short metricDataFormat, ushort numOfLongVerMetrics)
	{
		Version = version;
		VertTypoAscender = vertTypoAscender;
		VertTypoDescender = vertTypoDescender;
		VertTypoLineGap = vertTypoLineGap;
		AdvanceHeightMax = advanceHeightMax;
		MinTopSideBearing = minTopSideBearing;
		MinBottomSideBearing = minBottomSideBearing;
		YMaxExtent = yMaxExtent;
		CaretSlopeRise = caretSlopeRise;
		CaretSlopeRun = caretSlopeRun;
		CaretOffset = caretOffset;
		Reserved1 = reserved1;
		Reserved2 = reserved2;
		Reserved3 = reserved3;
		Reserved4 = reserved4;
		MetricDataFormat = metricDataFormat;
		NumOfLongVerMetrics = numOfLongVerMetrics;
	}
}