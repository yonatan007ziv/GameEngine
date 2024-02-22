using GameEngine.Core.Components.TrueTypeFont.Tables;
using System.Text;

namespace GameEngine.Core.Components.TrueTypeFont;

internal class TTFTableParser
{
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
			binaryReader.BaseStream.Seek(stringOffset + nameRecord[i].offset, SeekOrigin.Begin);
			byte[] readBytes = binaryReader.ReadBytes(nameRecord[i].length);
			readBytes = readBytes.Reverse().ToArray();
			name[i] = Encoding.UTF8.GetString(readBytes);
		}

		return new TTFName(format, count, stringOffset, nameRecord, name);
	}

	public TTFLoca ReadLoca(BigEndianBinaryReader binaryReader, TTFTableInfo locaInfo)
	{
		return new TTFLoca();
	}

	public TTFGlyf ReadGlyf(BigEndianBinaryReader binaryReader, TTFTableInfo glyfInfo)
	{
		return new TTFGlyf();
	}

	public TTFCmap ReadCmap(BigEndianBinaryReader binaryReader, TTFTableInfo cmapInfo)
	{
		binaryReader.BaseStream.Seek(cmapInfo.offset, SeekOrigin.Begin);

		ushort version = binaryReader.ReadUInt16();
		ushort numberSubtables = binaryReader.ReadUInt16();

		TTFCmap.Subtable[] subtables = new TTFCmap.Subtable[numberSubtables];
		for (int i = 0; i < numberSubtables; i++)
		{
			ushort platformID = binaryReader.ReadUInt16();
			ushort platformSpecificID = binaryReader.ReadUInt16();
			ushort offset = binaryReader.ReadUInt16();
			subtables[i] = new TTFCmap.Subtable(platformID, platformSpecificID, offset);
		}

		return new TTFCmap();
	}

	public TTFHhea ReadHhea(BigEndianBinaryReader binaryReader, TTFTableInfo hheaInfo)
	{
		throw new NotImplementedException();
	}

	public TTFVhea ReadVhea(BigEndianBinaryReader binaryReader, TTFTableInfo vheaInfo)
	{
		throw new NotImplementedException();
	}

	public TTFHmtx ReadHmtx(BigEndianBinaryReader binaryReader, TTFTableInfo hmtxInfo)
	{
		throw new NotImplementedException();
	}

	public TTFVmtx ReadVmtx(BigEndianBinaryReader binaryReader, TTFTableInfo vmtxInfo)
	{
		throw new NotImplementedException();
	}

	public TTFMaxp ReadMaxp(BigEndianBinaryReader binaryReader, TTFTableInfo maxpInfo)
	{
		throw new NotImplementedException();
	}
}