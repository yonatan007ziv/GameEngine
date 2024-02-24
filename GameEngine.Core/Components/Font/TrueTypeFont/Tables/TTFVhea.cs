namespace GameEngine.Core.Components.Font.TrueTypeFont.Tables;

internal class TTFVhea
{
	private float version;
	private short vertTypoAscender;
	private short vertTypoDescender;
	private short vertTypoLineGap;
	private short advanceHeightMax;
	private short minTopSideBearing;
	private short minBottomSideBearing;
	private short yMaxExtent;
	private short caretSlopeRise;
	private short caretSlopeRun;
	private short caretOffset;
	private short reserved1;
	private short reserved2;
	private short reserved3;
	private short reserved4;
	private short metricDataFormat;
	private ushort numOfLongVerMetrics;

	public TTFVhea(float version, short vertTypoAscender, short vertTypoDescender, short vertTypoLineGap, short advanceHeightMax, short minTopSideBearing, short minBottomSideBearing, short yMaxExtent, short caretSlopeRise, short caretSlopeRun, short caretOffset, short reserved1, short reserved2, short reserved3, short reserved4, short metricDataFormat, ushort numOfLongVerMetrics)
	{
		this.version = version;
		this.vertTypoAscender = vertTypoAscender;
		this.vertTypoDescender = vertTypoDescender;
		this.vertTypoLineGap = vertTypoLineGap;
		this.advanceHeightMax = advanceHeightMax;
		this.minTopSideBearing = minTopSideBearing;
		this.minBottomSideBearing = minBottomSideBearing;
		this.yMaxExtent = yMaxExtent;
		this.caretSlopeRise = caretSlopeRise;
		this.caretSlopeRun = caretSlopeRun;
		this.caretOffset = caretOffset;
		this.reserved1 = reserved1;
		this.reserved2 = reserved2;
		this.reserved3 = reserved3;
		this.reserved4 = reserved4;
		this.metricDataFormat = metricDataFormat;
		this.numOfLongVerMetrics = numOfLongVerMetrics;
	}
}