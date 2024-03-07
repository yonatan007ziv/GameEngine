using GameEngine.Core.Components;
using GameEngine.Core.Components.Font;
using GameEngine.Core.Components.Font.TrueTypeFont;
using GameEngine.Core.Components.Font.TrueTypeFont.Tables;
using GameEngine.Core.SharedServices.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text;

namespace GameEngine.Core.SharedServices.Implementations.FileReaders;

public class TrueTypeFontFileReader : IFileReader<Font>
{
	private readonly ILogger logger;
	private readonly IResourceDiscoverer resourceDiscoverer;

	public TrueTypeFontFileReader(ILogger logger, IResourceDiscoverer resourceDiscoverer)
	{
		this.logger = logger;
		this.resourceDiscoverer = resourceDiscoverer;
	}

	private static uint CalculateTableChecksum(BigEndianBinaryReader file, uint offset, uint length)
	{
		// Go to specified offset
		file.BaseStream.Seek(offset, SeekOrigin.Begin);

		// Calculate number of 32-bit chunks
		uint nlongs = (length + 3) / 4;

		uint sum = 0;
		while (nlongs-- > 0)
			sum += file.ReadUInt32();

		return sum;
	}

	public bool ReadFile(string fontName, out Font result)
	{
		if (!resourceDiscoverer.ResourceExists(fontName))
		{
			result = default!;
			return false;
		}

		using FileStream fs = File.Open(resourceDiscoverer.GetResourcePath(fontName), FileMode.Open);
		using BigEndianBinaryReader binaryReader = new BigEndianBinaryReader(fs);

		// Read BinSrchHeader
		uint scalarType = binaryReader.ReadUInt32();
		ushort numTables = binaryReader.ReadUInt16();
		ushort searchRange = binaryReader.ReadUInt16();
		ushort entrySelector = binaryReader.ReadUInt16();
		ushort rangeShift = binaryReader.ReadUInt16();

		// Read tables
		List<TTFTableInfo> tableInfos = new List<TTFTableInfo>();
		for (int i = 0; i < numTables; i++)
		{

			string tag = new string(Encoding.ASCII.GetChars(binaryReader.ReadBytes(4)));
			uint checksum = binaryReader.ReadUInt32();
			uint offset = binaryReader.ReadUInt32();
			uint length = binaryReader.ReadUInt32();

			tableInfos.Add(new TTFTableInfo(tag, checksum, offset, length));
		}

		// Match table infos
		TTFTableInfo headInfo = default, nameInfo = default, locaInfo = default, glyfInfo = default, cmapInfo = default, hheaInfo = default, vheaInfo = default, hmtxInfo = default, vmtxInfo = default, maxpInfo = default;
		foreach (TTFTableInfo tableInfo in tableInfos)
		{
			// Read head name loca glyf cmap hhea vhea hmtx vmtx maxp
			switch (tableInfo.tag)
			{
				case "head":
					headInfo = tableInfo;
					break;
				case "name":
					nameInfo = tableInfo;
					break;
				case "loca":
					locaInfo = tableInfo;
					break;
				case "glyf":
					glyfInfo = tableInfo;
					break;
				case "cmap":
					cmapInfo = tableInfo;
					break;
				case "hhea":
					hheaInfo = tableInfo;
					break;
				case "vhea":
					vheaInfo = tableInfo;
					break;
				case "hmtx":
					hmtxInfo = tableInfo;
					break;
				case "vmtx":
					vmtxInfo = tableInfo;
					break;
				case "maxp":
					maxpInfo = tableInfo;
					break;
			}
		}

		// Compare checksums
		foreach (TTFTableInfo tableInfo in tableInfos)
		{
			if (tableInfo.tag == "head")
				continue;

			uint checksum;
			if ((checksum = CalculateTableChecksum(binaryReader, tableInfo.offset, tableInfo.length)) != tableInfo.checksum)
			{
				logger.LogError("Checksum error while loading font! expected: {checksum}, got: {gotChecksum}", tableInfo.checksum, checksum);
				result = default!;
				return false;
			}
		}

		// TODO: Compare head checksum

		TTFTableParser tableParser = new TTFTableParser(logger);
		TTFHead head = tableParser.ReadHead(binaryReader, headInfo);
		TTFName name = tableParser.ReadName(binaryReader, nameInfo);
		TTFMaxp maxp = tableParser.ReadMaxp(binaryReader, maxpInfo);
		TTFLoca loca = tableParser.ReadLoca(head, maxp, binaryReader, locaInfo);
		TTFGlyf glyf = tableParser.ReadGlyf(maxp, loca, binaryReader, glyfInfo);
		TTFCmap cmap = tableParser.ReadCmap(binaryReader, cmapInfo);
		TTFHhea hhea = tableParser.ReadHhea(binaryReader, hheaInfo);
		TTFVhea vhea = tableParser.ReadVhea(binaryReader, vheaInfo);
		TTFHmtx hmtx = tableParser.ReadHmtx(hhea, binaryReader, hmtxInfo);
		TTFVmtx vmtx = tableParser.ReadVmtx(binaryReader, vmtxInfo);

		result = new Font(head, name, loca, glyf, cmap, hhea, vhea, hmtx, vmtx, maxp);
		return true;
	}

	// Parsers
}