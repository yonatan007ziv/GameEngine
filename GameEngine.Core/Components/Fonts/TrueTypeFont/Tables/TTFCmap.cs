namespace GameEngine.Core.Components.Fonts.TrueTypeFont.Tables;

internal class TTFCmap
{
	public ushort Version { get; }
	public ushort NumberSubtables { get; }
	public Subtable[] Subtables { get; }

	public TTFCmap(ushort version, ushort numberSubtables, Subtable[] subtables)
	{
		Version = version;
		NumberSubtables = numberSubtables;
		Subtables = subtables;
	}

	internal class Subtable
	{
		public ushort PlatformID { get; }
		public ushort PlatformSpecificID { get; }
		public uint Offset { get; }

		public Subtable(ushort platformID, ushort platformSpecificID, uint offset)
		{
			PlatformID = platformID;
			PlatformSpecificID = platformSpecificID;
			Offset = offset;
		}
	}
	internal class SubtableFormat0 : Subtable
	{
		public ushort Format { get; }
		public ushort Length { get; }
		public ushort Language { get; }
		public byte[] GlyphIndexArray { get; }

		public SubtableFormat0(ushort platformID, ushort platformSpecificID, uint offset, ushort format, ushort length, ushort language, byte[] glyphIndexArray)
			: base(platformID, platformSpecificID, offset)
		{
			Format = format;
			Length = length;
			Language = language;
			GlyphIndexArray = glyphIndexArray;
		}
	}
	internal class SubtableFormat2 : Subtable
	{
		internal struct SubHeader
		{
			public ushort FirstCode { get; }
			public ushort EntryCount { get; }
			public short IdDelta { get; }
			public ushort IdRangeOffset { get; }
		}

		public ushort Format { get; }
		public ushort Length { get; }
		public ushort Language { get; }
		public ushort[] SubHeaderKeys { get; }
		public SubHeader[] SubHeaders { get; }
		public ushort[] GlyphIndexArray { get; }

		public SubtableFormat2(ushort platformID, ushort platformSpecificID, uint offset, ushort format, ushort length, ushort language, ushort[] subHeaderKeys, SubHeader[] subHeaders, ushort[] glyphIndexArray)
			: base(platformID, platformSpecificID, offset)
		{
			Format = format;
			Length = length;
			Language = language;
			SubHeaderKeys = subHeaderKeys;
			SubHeaders = subHeaders;
			GlyphIndexArray = glyphIndexArray;
		}
	}
	internal class SubtableFormat4 : Subtable
	{
		public ushort Format { get; }
		public ushort Length { get; }
		public ushort Language { get; }
		public ushort SegCountX2 { get; }
		public ushort SearchRange { get; }
		public ushort EntrySelector { get; }
		public ushort RangeShift { get; }
		public ushort[] EndCode { get; }
		public ushort ReservedPad { get; }
		public ushort[] StartCode { get; }
		public ushort[] IdDelta { get; }
		public ushort[] IdRangeOffset { get; }
		public ushort[] GlyphIndexArray { get; }

		public SubtableFormat4(ushort platformID, ushort platformSpecificID, uint offset, ushort format, ushort length, ushort language, ushort segCountX2, ushort searchRange, ushort entrySelector, ushort rangeShift, ushort[] endCode, ushort reservedPad, ushort[] startCode, ushort[] idDelta, ushort[] idRangeOffset, ushort[] glyphIndexArray)
			: base(platformID, platformSpecificID, offset)
		{
			// does it exist in the subtable? Format = format;
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
	internal class SubtableFormat6 : Subtable
	{
		public ushort Format { get; }
		public ushort Length { get; }
		public ushort Language { get; }
		public ushort FirstCode { get; }
		public ushort EntryCount { get; }
		public ushort[] GlyphIndexArray { get; }

		public SubtableFormat6(ushort platformID, ushort platformSpecificID, uint offset, ushort format, ushort length, ushort language, ushort firstCode, ushort entryCount, ushort[] glyphIndexArray)
			: base(platformID, platformSpecificID, offset)
		{
			Format = format;
			Length = length;
			Language = language;
			FirstCode = firstCode;
			EntryCount = entryCount;
			GlyphIndexArray = glyphIndexArray;
		}
	}
	internal class SubtableFormat8 : Subtable
	{
		internal struct Group
		{
			public uint StartCharCode { get; }
			public uint EndCharCode { get; }
			public uint StartGlyphCode { get; }
		}

		public ushort Format { get; }
		public ushort Reserved { get; }
		public uint Length { get; }
		public uint Language { get; }
		public byte[] Is32 { get; }
		public uint NGroups { get; }
		public Group[] Groups { get; }

		public SubtableFormat8(ushort platformID, ushort platformSpecificID, uint offset, ushort format, ushort reserved, uint length, uint language, byte[] is32, uint nGroups, Group[] groups)
			: base(platformID, platformSpecificID, offset)
		{
			Format = format;
			Reserved = reserved;
			Length = length;
			Language = language;
			Is32 = is32;
			NGroups = nGroups;
			Groups = groups;
		}
	}
	internal class SubtableFormat10 : Subtable
	{
		public ushort Format { get; }
		public ushort Reserved { get; }
		public uint Length { get; }
		public uint Language { get; }
		public uint StartCharCode { get; }
		public uint NumChars { get; }
		public ushort[] Glyphs { get; }

		public SubtableFormat10(ushort platformID, ushort platformSpecificID, uint offset, ushort format, ushort reserved, uint length, uint language, uint startCharCode, uint numChars, ushort[] glyphs)
			: base(platformID, platformSpecificID, offset)
		{
			Format = format;
			Reserved = reserved;
			Length = length;
			Language = language;
			StartCharCode = startCharCode;
			NumChars = numChars;
			Glyphs = glyphs;
		}
	}
	internal class SubtableFormat12 : Subtable
	{
		public SubtableFormat12(ushort platformID, ushort platformSpecificID, uint offset) : base(platformID, platformSpecificID, offset)
		{
		}
	} // Not supported
	internal class SubtableFormat13 : Subtable
	{
		public SubtableFormat13(ushort platformID, ushort platformSpecificID, uint offset) : base(platformID, platformSpecificID, offset)
		{
		}
	} // Not supported
	internal class SubtableFormat14 : Subtable
	{
		public SubtableFormat14(ushort platformID, ushort platformSpecificID, uint offset) : base(platformID, platformSpecificID, offset)
		{
		}
	} // Not supported
}