namespace GameEngine.Core.Components.Font.TrueTypeFont.Tables;

internal class TTFCmap
{
	internal struct SubtableFormat4
	{
		public ushort PlatformID { get; }
		public ushort PlatformSpecificID { get; }
		public uint Offset { get; }
		public ushort Length { get; }
		public ushort Language { get; }
		public ushort SegCountX2 { get; }
		public ushort SearchRange { get; }
		public ushort EntrySelector { get; }
		public ushort RangeShift { get; }
		public ushort EndCode { get; }
		public ushort ReservedPad { get; }
		public ushort StartCode { get; }
		public ushort IdDelta { get; }
		public ushort IdRangeOffset { get; }
		public ushort GlyphIndexArray { get; }

		public SubtableFormat4(ushort platformID, ushort platformSpecificID, uint offset, ushort format, ushort length, ushort language, ushort segCountX2, ushort searchRange, ushort entrySelector, ushort rangeShift, ushort endCode, ushort reservedPad, ushort startCode, ushort idDelta, ushort idRangeOffset, ushort glyphIndexArray)
		{
			PlatformID = platformID;
			PlatformSpecificID = platformSpecificID;
			Offset = offset;
			Length = length;
			Language = language;
			SegCountX2 = segCountX2;
			SearchRange = searchRange;
			EntrySelector = entrySelector;
			RangeShift = rangeShift;
			EndCode = endCode;
			ReservedPad = reservedPad;
			StartCode = startCode;
			IdDelta = idDelta;
			IdRangeOffset = idRangeOffset;
			GlyphIndexArray = glyphIndexArray;
		}
	}
}