using GameEngine.Core.Components.Fonts.TrueTypeFont.Tables;
using Microsoft.Extensions.Logging;
using System.Text;

namespace GameEngine.Core.Components.Fonts.TrueTypeFont;

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

		TTFName.NameRecordStruct[] nameRecord = new TTFName.NameRecordStruct[count];
		for (int i = 0; i < count; i++)
		{
			ushort platformID = binaryReader.ReadUInt16();
			ushort platformSpecificID = binaryReader.ReadUInt16();
			ushort languageID = binaryReader.ReadUInt16();
			ushort nameID = binaryReader.ReadUInt16();
			ushort length = binaryReader.ReadUInt16();
			ushort offset = binaryReader.ReadUInt16();
			nameRecord[i] = new TTFName.NameRecordStruct(platformID, platformSpecificID, languageID, nameID, length, offset);
		}

		string[] name = new string[count];
		for (int i = 0; i < count; i++)
		{
			binaryReader.BaseStream.Seek(nameInfo.offset + stringOffset + nameRecord[i].offset, SeekOrigin.Begin);
			byte[] readBytes = binaryReader.ReadBytesBigEndian(nameRecord[i].length);
			name[i] = Encoding.BigEndianUnicode.GetString(readBytes).Replace("\0", "");
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
			byte[] flagsSimple = null!;
			short[] xCoordinates = null!;
			short[] yCoordinates = null!;

			// Compound glyph definitions
			ushort flagsCompound = 0;
			ushort glyphIndex = 0;
			int argument1 = 0;
			int argument2 = 0;
			int transformationOption1 = 0;
			int transformationOption2 = 0;
			int transformationOption3 = 0;
			int transformationOption4 = 0;

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

				int numOfPoints = numberOfContours == 0 ? 0 : endPtsOfContours.Max() + 1;

				flagsSimple = new byte[numOfPoints];
				xCoordinates = new short[numOfPoints];
				yCoordinates = new short[numOfPoints];

				// Read flags
				j = -1;
				while (++j < numOfPoints)
				{
					flagsSimple[j] = ReadUInt8(binaryReader);
					if ((flagsSimple[j] & (int)TTFGlyf.SIMPLE_FLAGS.REPEAT) != 0)
					{
						byte repeatCount = ReadUInt8(binaryReader);
						for (int k = 0; k < repeatCount; k++)
						{
							flagsSimple[j + 1] = flagsSimple[j - k];
							j++;
						}
					}
				}

				// Read x coordinates
				j = -1;
				short posX = 0;
				while (++j < numOfPoints)
				{
					if ((flagsSimple[j] & (int)TTFGlyf.SIMPLE_FLAGS.X_IS_BYTE) != 0)
					{
						if ((flagsSimple[j] & (int)TTFGlyf.SIMPLE_FLAGS.X_DELTA) != 0)
							xCoordinates[j] = posX += ReadUInt8(binaryReader);
						else
							xCoordinates[j] = posX += (short)(ReadUInt8(binaryReader) * -1);
					}
					else
					{
						if ((flagsSimple[j] & (int)TTFGlyf.SIMPLE_FLAGS.X_DELTA) != 0)
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
					if ((flagsSimple[j] & (int)TTFGlyf.SIMPLE_FLAGS.Y_IS_BYTE) != 0)
					{
						if ((flagsSimple[j] & (int)TTFGlyf.SIMPLE_FLAGS.Y_DELTA) != 0)
							yCoordinates[j] = posY += ReadUInt8(binaryReader);
						else
							yCoordinates[j] = posY += (short)(ReadUInt8(binaryReader) * -1);
					}
					else
					{
						if ((flagsSimple[j] & (int)TTFGlyf.SIMPLE_FLAGS.Y_DELTA) != 0)
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
			glyphs[i] = new TTFGlyf.GlyfData(numberOfContours, xMin, yMin, xMax, yMax, endPtsOfContours, instructionLength, instructions, flagsSimple, xCoordinates, yCoordinates, flagsCompound, glyphIndex, argument1, argument2, transformationOption1, transformationOption2, transformationOption3, transformationOption4);
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
			ushort platformID = binaryReader.ReadUInt16();
			ushort platformSpecificID = binaryReader.ReadUInt16();
			uint offset = binaryReader.ReadUInt32();

			switch (platformID)
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

			subtables[i] = new TTFCmap.SubtableFormat4(platformID, platformSpecificID, offset, format, length, language, segCountX2, searchRange, entrySelector, rangeShift, endCode, reservedPad, startCode, idDelta, idRangeOffset, glyphIndexArray);
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
		short caretSlopeRise = ReadInt16(binaryReader);
		short caretSlopeRun = ReadInt16(binaryReader);
		short caretOffset = ReadFWord(binaryReader);
		short reserved1 = ReadInt16(binaryReader);
		short reserved2 = ReadInt16(binaryReader);
		short reserved3 = ReadInt16(binaryReader);
		short reserved4 = ReadInt16(binaryReader);
		short metricDataFormat = ReadInt16(binaryReader);
		ushort numOfLongHorMetrics = ReadUInt16(binaryReader);

		return new TTFHhea(version, ascent, descent, lineGap, advanceWidthMax, minLeftSideBearing, minRightSideBearing, xMaxExtent, caretSlopeRise, caretSlopeRun, caretOffset, reserved1, reserved2, reserved3, reserved4, metricDataFormat, numOfLongHorMetrics);
	}

	public TTFVhea ReadVhea(BigEndianBinaryReader binaryReader, TTFTableInfo vheaInfo)
	{
		binaryReader.BaseStream.Seek(vheaInfo.offset, SeekOrigin.Begin);

		float version = ReadFixed1616(binaryReader);
		short vertTypoAscender = ReadInt16(binaryReader);
		short vertTypoDescender = ReadInt16(binaryReader);
		short vertTypoLineGap = ReadInt16(binaryReader);
		short advanceHeightMax = ReadInt16(binaryReader);
		short minTopSideBearing = ReadInt16(binaryReader);
		short minBottomSideBearing = ReadInt16(binaryReader);
		short yMaxExtent = ReadInt16(binaryReader);
		short caretSlopeRise = ReadInt16(binaryReader);
		short caretSlopeRun = ReadInt16(binaryReader);
		short caretOffset = ReadInt16(binaryReader);
		short reserved1 = ReadInt16(binaryReader);
		short reserved2 = ReadInt16(binaryReader);
		short reserved3 = ReadInt16(binaryReader);
		short reserved4 = ReadInt16(binaryReader);
		short metricDataFormat = ReadInt16(binaryReader);
		ushort numOfLongVerMetrics = ReadUInt16(binaryReader);

		return new TTFVhea(version, vertTypoAscender, vertTypoDescender, vertTypoLineGap, advanceHeightMax, minTopSideBearing, minBottomSideBearing, yMaxExtent, caretSlopeRise, caretSlopeRun, caretOffset, reserved1, reserved2, reserved3, reserved4, metricDataFormat, numOfLongVerMetrics);
	}

	public TTFHmtx ReadHmtx(TTFHhea hhea, BigEndianBinaryReader binaryReader, TTFTableInfo hmtxInfo)
	{
		return new TTFHmtx();
	}

	public TTFVmtx ReadVmtx(BigEndianBinaryReader binaryReader, TTFTableInfo vmtxInfo)
	{
		return new TTFVmtx();
	}

	public TTFMaxp ReadMaxp(BigEndianBinaryReader binaryReader, TTFTableInfo maxpInfo)
	{
		binaryReader.BaseStream.Seek(maxpInfo.offset, SeekOrigin.Begin);

		float version = ReadFixed32(binaryReader);
		ushort numGlyphs = ReadUInt16(binaryReader);
		ushort maxPoints = ReadUInt16(binaryReader);
		ushort maxContours = ReadUInt16(binaryReader);
		ushort maxComponentPoints = ReadUInt16(binaryReader);
		ushort maxComponentContours = ReadUInt16(binaryReader);
		ushort maxZones = ReadUInt16(binaryReader);
		ushort maxTwilightPoints = ReadUInt16(binaryReader);
		ushort maxStorage = ReadUInt16(binaryReader);
		ushort maxFunctionDefs = ReadUInt16(binaryReader);
		ushort maxInstructionDefs = ReadUInt16(binaryReader);
		ushort maxStackElements = ReadUInt16(binaryReader);
		ushort maxSizeOfInstructions = ReadUInt16(binaryReader);
		ushort maxComponentElements = ReadUInt16(binaryReader);
		ushort maxComponentDepth = ReadUInt16(binaryReader);

		return new TTFMaxp(version, numGlyphs, maxPoints, maxContours, maxComponentPoints, maxComponentContours, maxZones, maxTwilightPoints, maxStorage, maxFunctionDefs, maxInstructionDefs, maxStackElements, maxSizeOfInstructions, maxComponentElements, maxComponentDepth);
	}

	// Reads a 16.16 fixed-point number
	private static float ReadFixed1616(BigEndianBinaryReader binaryReader)
	{
		int integer = binaryReader.ReadInt32();
		return (integer >> 16) + (integer & 0xFFFF) / 65536f;
	}
	private static float ReadFixed32(BigEndianBinaryReader binaryReader) => binaryReader.ReadFixed32();
	private static short ReadFWord(BigEndianBinaryReader binaryReader) => binaryReader.ReadInt16();
	private static ushort ReadUFWord(BigEndianBinaryReader binaryReader) => binaryReader.ReadUInt16();
	private static sbyte ReadInt8(BigEndianBinaryReader binaryReader) => binaryReader.ReadInt8();
	private static short ReadInt16(BigEndianBinaryReader binaryReader) => binaryReader.ReadInt16();
	private static int ReadInt32(BigEndianBinaryReader binaryReader) => binaryReader.ReadInt16();
	private static byte ReadUInt8(BigEndianBinaryReader binaryReader) => binaryReader.ReadUInt8();
	private static ushort ReadUInt16(BigEndianBinaryReader binaryReader) => binaryReader.ReadUInt16();
	private static uint ReadUInt32(BigEndianBinaryReader binaryReader) => binaryReader.ReadUInt32();
}