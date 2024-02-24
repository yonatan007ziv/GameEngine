using GameEngine.Core.Components.TrueTypeFont.Tables;
using Microsoft.Extensions.Logging;
using System.Text;

namespace GameEngine.Core.Components.TrueTypeFont;

internal class TTFTableParser
{
	private readonly ILogger logger;

	public TTFTableParser(ILogger logger)
	{
		this.logger = logger;
	}

	public TTFHead ReadHead(BigEndianBinaryReader binaryReader, TTFTableInfo headInfo)
	{
		binaryReader.BaseStream.Seek(headInfo.offset, SeekOrigin.Begin);

		// Read 16.16 fixed-point number, version
		int versionInteger = binaryReader.ReadInt32();
		float version = (versionInteger >> 16) + (versionInteger & 0xFFFF) / 65536f;

		// Read 16.16 fixed-point number, fontRevision
		int fontRevisionInteger = binaryReader.ReadInt32();
		float fontRevision = (fontRevisionInteger >> 16) + (fontRevisionInteger & 0xFFFF) / 65536f;

		uint checkSumAdjustment = binaryReader.ReadUInt32();
		uint magicNumber = binaryReader.ReadUInt32();
		ushort flags = binaryReader.ReadUInt16();
		ushort unitsPerEm = binaryReader.ReadUInt16();
		long created = binaryReader.ReadInt64();
		long modified = binaryReader.ReadInt64();
		short xMin = binaryReader.ReadInt16();
		short yMin = binaryReader.ReadInt16();
		short xMax = binaryReader.ReadInt16();
		short yMax = binaryReader.ReadInt16();
		ushort macStyle = binaryReader.ReadUInt16();
		ushort lowestRecPPEM = binaryReader.ReadUInt16();
		short fontDirectionHint = binaryReader.ReadInt16();
		short indexToLocFormat = binaryReader.ReadInt16();
		short glyphDataFormat = binaryReader.ReadInt16();

		return new TTFHead(version, fontRevision, checkSumAdjustment, magicNumber, flags, unitsPerEm, created, modified, xMin, yMin, xMax, yMax, macStyle, lowestRecPPEM, fontDirectionHint, indexToLocFormat, glyphDataFormat);
	}

	public TTFName ReadName(BigEndianBinaryReader binaryReader, TTFTableInfo nameInfo)
	{
		binaryReader.BaseStream.Seek(nameInfo.offset, SeekOrigin.Begin);

		ushort format = binaryReader.ReadUInt16();
		ushort count = binaryReader.ReadUInt16();
		ushort stringOffset = binaryReader.ReadUInt16();

		TTFName.NameRecord[] nameRecord = new TTFName.NameRecord[count];
		for (int i = 0; i < count; i++)
		{
			ushort platformID = binaryReader.ReadUInt16();
			ushort platformSpecificID = binaryReader.ReadUInt16();
			ushort languageID = binaryReader.ReadUInt16();
			ushort nameID = binaryReader.ReadUInt16();
			ushort length = binaryReader.ReadUInt16();
			ushort offset = binaryReader.ReadUInt16();
			nameRecord[i] = new TTFName.NameRecord(platformID, platformSpecificID, languageID, nameID, length, offset);
		}

		string[] name = new string[count];
		for (int i = 0; i < count; i++)
		{
			binaryReader.BaseStream.Seek(/* relative offset? */ nameRecord[i].offset, SeekOrigin.Begin);
			byte[] readBytes = binaryReader.ReadBytes(nameRecord[i].length);
			readBytes = readBytes.Reverse().ToArray();
			name[i] = Encoding.UTF8.GetString(readBytes);
		}

		return new TTFName(format, count, stringOffset, nameRecord, name);
	}

	public TTFLoca ReadLoca(TTFHead head, TTFMaxp maxp, BigEndianBinaryReader binaryReader, TTFTableInfo locaInfo)
	{
		binaryReader.BaseStream.Seek(locaInfo.offset, SeekOrigin.Begin);

		uint[] offsets = new uint[maxp.NumGlyphs + 1];
		if (head.IndexToLocFormat == 0)
			for (int i = 0; i < maxp.NumGlyphs + 1; i++)
				offsets[i] = ReadUInt16(binaryReader) * 2u;
		if (head.IndexToLocFormat == 1)
			for (int i = 0; i < maxp.NumGlyphs + 1; i++)
				offsets[i] = ReadUInt32(binaryReader);
		return new TTFLoca(offsets);
	}

	public TTFGlyf ReadGlyf(TTFMaxp maxp, TTFLoca loca, BigEndianBinaryReader binaryReader, TTFTableInfo glyfInfo)
	{
		TTFGlyf.GlyfData[] glyphs = new TTFGlyf.GlyfData[maxp.NumGlyphs + 1];
		for (int i = 0; i < maxp.NumGlyphs + 1; i++)
		{
			binaryReader.BaseStream.Seek(glyfInfo.offset + loca.Offsets[i], SeekOrigin.Begin);

			short numberOfContours = ReadInt16(binaryReader);
			short xMin = ReadFWord(binaryReader);
			short yMin = ReadFWord(binaryReader);
			short xMax = ReadFWord(binaryReader);
			short yMax = ReadFWord(binaryReader);

			// Single glyph definitions
			ushort[] endPtsOfContours = null!;
			ushort instructionLength = 0;
			byte[] instructions = null!;
			byte[] flags = null!;
			short[] xCoordinates = null!;
			short[] yCoordinates = null!;

			// Compound glyph definitions


			if (numberOfContours >= 0)
			{
				int j;
				// Single glyph
				endPtsOfContours = new ushort[numberOfContours];
				for (j = 0; j < numberOfContours; j++)
					endPtsOfContours[j] = ReadUInt16(binaryReader);

				instructionLength = ReadUInt16(binaryReader);

				instructions = new byte[instructionLength];
				for (j = 0; j < instructionLength; j++)
					instructions[j] = ReadUInt8(binaryReader);

				int numOfPoints = numberOfContours == 0 ? 0 : (endPtsOfContours.Max() + 1);

				flags = new byte[numOfPoints];
				xCoordinates = new short[numOfPoints];
				yCoordinates = new short[numOfPoints];

				// Read flags
				j = -1;
				while (++j < numOfPoints)
				{
					flags[j] = ReadUInt8(binaryReader);
					if ((flags[j] & (int)TTFGlyf.SIMPLE_FLAGS.REPEAT) != 0)
					{
						byte repeatCount = ReadUInt8(binaryReader);
						for (int k = 0; k < repeatCount; k++)
							flags[j + k] = flags[j];
						j += repeatCount;
					}
				}

				if (i == 5)
				{

				}

				// Read x coordinates
				j = -1;
				short posX = 0;
				while (++j < numOfPoints)
				{
					if ((flags[j] & (int)TTFGlyf.SIMPLE_FLAGS.X_IS_BYTE) != 0)
					{
						if ((flags[j] & (int)TTFGlyf.SIMPLE_FLAGS.X_DELTA) != 0)
							xCoordinates[j] = posX += ReadUInt8(binaryReader);
						else
							xCoordinates[j] = posX += (short)(ReadUInt8(binaryReader) * -1);
					}
					else
					{
						if ((flags[j] & (int)TTFGlyf.SIMPLE_FLAGS.X_DELTA) != 0)
							xCoordinates[j] = posX;
						else
							xCoordinates[j] = posX += ReadInt16(binaryReader);
					}
				}

				// Read y coordinates
				j = -1;
				short posY = 0;
				while (++j < numOfPoints)
				{
					if ((flags[j] & (int)TTFGlyf.SIMPLE_FLAGS.Y_IS_BYTE) != 0)
					{
						if ((flags[j] & (int)TTFGlyf.SIMPLE_FLAGS.Y_DELTA) != 0)
							yCoordinates[j] = posY += ReadUInt8(binaryReader);
						else
							yCoordinates[j] = posY += (short)(ReadUInt8(binaryReader) * -1);
					}
					else
					{
						if ((flags[j] & (int)TTFGlyf.SIMPLE_FLAGS.Y_DELTA) != 0)
							yCoordinates[j] = posY;
						else
							yCoordinates[j] = posY += ReadInt16(binaryReader);
					}
				}
			}
			else
			{
				// Compound glyph

			}
			glyphs[i] = new TTFGlyf.GlyfData(numberOfContours, xMin, yMin, xMax, yMax, endPtsOfContours, instructionLength, instructions, flags, xCoordinates, yCoordinates);
		}

		return new TTFGlyf(glyphs);
	}

	#region CMAP
	public TTFCmap ReadCmap(BigEndianBinaryReader binaryReader, TTFTableInfo cmapInfo)
	{
		binaryReader.BaseStream.Seek(cmapInfo.offset, SeekOrigin.Begin);

		ushort version = binaryReader.ReadUInt16();
		ushort numberSubtables = binaryReader.ReadUInt16();

		for (int i = 0; i < numberSubtables; i++)
		{
			ushort format = binaryReader.ReadUInt16();

			switch (format)
			{
				default:
					logger.LogError("TTF: Invalid CMAP format");
					break;
				case 0:
					break;
				case 2:
					break;
				case 4:
					return ReadCmapFormat4(binaryReader, version, numberSubtables);
				case 6:
					break;
				case 8:
					break;
				case 10:
					break;
				case 12:
					break;
				case 13:
					break;
				case 14:
					break;
			}
		}

		return new TTFCmap();
	}
	public TTFCmap ReadCmapFormat4(BigEndianBinaryReader binaryReader, ushort version, ushort numberSubtables)
	{
		TTFCmap.SubtableFormat4[] subtables = new TTFCmap.SubtableFormat4[numberSubtables];
		for (int i = 0; i < numberSubtables; i++)
		{
			ushort platformID = binaryReader.ReadUInt16();
			ushort platformSpecificID = binaryReader.ReadUInt16();
			ushort offset = binaryReader.ReadUInt16();

			ushort format = binaryReader.ReadUInt16();
			ushort length = binaryReader.ReadUInt16();
			ushort language = binaryReader.ReadUInt16();
			ushort segCountX2 = binaryReader.ReadUInt16();
			ushort searchRange = binaryReader.ReadUInt16();
			ushort entrySelector = binaryReader.ReadUInt16();
			ushort rangeShift = binaryReader.ReadUInt16();
			// BS TREE
			ushort endCode = binaryReader.ReadUInt16();
			ushort reservedPad = binaryReader.ReadUInt16();
			// BS TREE
			ushort startCode = binaryReader.ReadUInt16();
			// BS TREE
			ushort idDelta = binaryReader.ReadUInt16();
			// BS TREE
			ushort idRangeOffset = binaryReader.ReadUInt16();
			ushort glyphIndexArray = binaryReader.ReadUInt16();

			subtables[i] = new TTFCmap.SubtableFormat4(platformID, platformSpecificID, offset, format, length, language, segCountX2, searchRange , entrySelector, rangeShift, endCode, reservedPad, startCode, idDelta, idRangeOffset, glyphIndexArray);
		}

		return new TTFCmap();
	}
	#endregion

	public TTFHhea ReadHhea(BigEndianBinaryReader binaryReader, TTFTableInfo hheaInfo)
	{
		binaryReader.BaseStream.Seek(hheaInfo.offset, SeekOrigin.Begin);

		float version = ReadFixed1616(binaryReader);
		short ascent = ReadFWord(binaryReader);
		short descent = ReadFWord(binaryReader);
		short lineGap = ReadFWord(binaryReader);
		ushort advanceWidthMax = ReadUFWord(binaryReader);
		short minLeftSideBearing = ReadFWord(binaryReader);
		short minRightSideBearing = ReadFWord(binaryReader);
		short xMaxExtent = ReadFWord(binaryReader);
		Int16 caretSlopeRise = ReadInt16(binaryReader);
		Int16 caretSlopeRun = ReadInt16(binaryReader);
		short caretOffset = ReadFWord(binaryReader);
		Int16 reserved1 = ReadInt16(binaryReader);
		Int16 reserved2 = ReadInt16(binaryReader);
		Int16 reserved3 = ReadInt16(binaryReader);
		Int16 reserved4 = ReadInt16(binaryReader);
		Int16 metricDataFormat = ReadInt16(binaryReader);
		UInt16 numOfLongHorMetrics = ReadUInt16(binaryReader);

		return new TTFHhea(version, ascent, descent, lineGap, advanceWidthMax, minLeftSideBearing, minRightSideBearing, xMaxExtent, caretSlopeRise, caretSlopeRun, caretOffset, reserved1, reserved2, reserved3, reserved4, metricDataFormat, numOfLongHorMetrics);
	}

	public TTFVhea ReadVhea(BigEndianBinaryReader binaryReader, TTFTableInfo vheaInfo)
	{
		binaryReader.BaseStream.Seek(vheaInfo.offset, SeekOrigin.Begin);

		float version = ReadFixed32(binaryReader);
		Int16 vertTypoAscender = ReadInt16(binaryReader);
		Int16 vertTypoDescender = ReadInt16(binaryReader);
		Int16 vertTypoLineGap = ReadInt16(binaryReader);
		Int16 advanceHeightMax = ReadInt16(binaryReader);
		Int16 minTopSideBearing = ReadInt16(binaryReader);
		Int16 minBottomSideBearing = ReadInt16(binaryReader);
		Int16 yMaxExtent = ReadInt16(binaryReader);
		Int16 caretSlopeRise = ReadInt16(binaryReader);
		Int16 caretSlopeRun = ReadInt16(binaryReader);
		Int16 caretOffset = ReadInt16(binaryReader);
		Int16 reserved1 = ReadInt16(binaryReader);
		Int16 reserved2 = ReadInt16(binaryReader);
		Int16 reserved3 = ReadInt16(binaryReader);
		Int16 reserved4 = ReadInt16(binaryReader);
		Int16 metricDataFormat = ReadInt16(binaryReader);
		UInt16 numOfLongVerMetrics = ReadUInt16(binaryReader);

		return new TTFVhea(version, vertTypoAscender, vertTypoDescender, vertTypoLineGap, advanceHeightMax, minTopSideBearing, minBottomSideBearing, yMaxExtent, caretSlopeRise, caretSlopeRun, caretOffset, reserved1, reserved2, reserved3, reserved4, metricDataFormat, numOfLongVerMetrics);
	}
	
	public TTFHmtx ReadHmtx(TTFHhea hhea, BigEndianBinaryReader binaryReader, TTFTableInfo hmtxInfo)
	{
		throw new NotImplementedException();
	}

	public TTFVmtx ReadVmtx(BigEndianBinaryReader binaryReader, TTFTableInfo vmtxInfo)
	{
		throw new NotImplementedException();
	}

	public TTFMaxp ReadMaxp(BigEndianBinaryReader binaryReader, TTFTableInfo maxpInfo)
	{
		binaryReader.BaseStream.Seek(maxpInfo.offset, SeekOrigin.Begin);

		float version = ReadFixed32(binaryReader);
		UInt16 numGlyphs = ReadUInt16(binaryReader);
		UInt16 maxPoints = ReadUInt16(binaryReader);
		UInt16 maxContours = ReadUInt16(binaryReader);
		UInt16 maxComponentPoints = ReadUInt16(binaryReader);
		UInt16 maxComponentContours = ReadUInt16(binaryReader);
		UInt16 maxZones = ReadUInt16(binaryReader);
		UInt16 maxTwilightPoints = ReadUInt16(binaryReader);
		UInt16 maxStorage = ReadUInt16(binaryReader);
		UInt16 maxFunctionDefs = ReadUInt16(binaryReader);
		UInt16 maxInstructionDefs = ReadUInt16(binaryReader);
		UInt16 maxStackElements = ReadUInt16(binaryReader);
		UInt16 maxSizeOfInstructions = ReadUInt16(binaryReader);
		UInt16 maxComponentElements = ReadUInt16(binaryReader);
		UInt16 maxComponentDepth = ReadUInt16(binaryReader);

		return new TTFMaxp(version, numGlyphs, maxPoints, maxContours, maxComponentPoints, maxComponentContours, maxZones, maxTwilightPoints, maxStorage, maxFunctionDefs, maxInstructionDefs, maxStackElements, maxSizeOfInstructions, maxComponentElements, maxComponentDepth);
	}

	// Reads a 16.16 fixed-point number
	private Single ReadFixed1616(BigEndianBinaryReader binaryReader)
	{
		int integer = binaryReader.ReadInt32();
		return (integer >> 16) + (integer & 0xFFFF) / 65536f;
	}
	private Single ReadFixed32(BigEndianBinaryReader binaryReader) => binaryReader.ReadFixed32();
	private short ReadFWord(BigEndianBinaryReader binaryReader) => binaryReader.ReadInt16();
	private ushort ReadUFWord(BigEndianBinaryReader binaryReader) => binaryReader.ReadUInt16();
	private SByte ReadInt8(BigEndianBinaryReader binaryReader) => binaryReader.ReadInt8();
	private Int16 ReadInt16(BigEndianBinaryReader binaryReader) => binaryReader.ReadInt16();
	private Int32 ReadInt32(BigEndianBinaryReader binaryReader) => binaryReader.ReadInt16();
	private Byte ReadUInt8(BigEndianBinaryReader binaryReader) => binaryReader.ReadUInt8();
	private UInt16 ReadUInt16(BigEndianBinaryReader binaryReader) => binaryReader.ReadUInt16();
	private UInt32 ReadUInt32(BigEndianBinaryReader binaryReader) => binaryReader.ReadUInt32();
}