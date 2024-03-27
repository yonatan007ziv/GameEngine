using GameEngine.Core.Components;
using GameEngine.Core.Components.Fonts;
using GameEngine.Core.Components.Fonts.TrueTypeFont;
using GameEngine.Core.Components.Fonts.TrueTypeFont.Tables;
using GameEngine.Core.SharedServices.Interfaces;
using GameEngine.Extensions;
using Microsoft.Extensions.Logging;
using System.Numerics;
using System.Text;

namespace GameEngine.Core.SharedServices.Implementations.FontParsers;

public class TrueTypeFontFileReader
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
		TTFCmap cmap = tableParser.ReadCmap(binaryReader, cmapInfo); //
		TTFHhea hhea = tableParser.ReadHhea(binaryReader, hheaInfo);
		TTFVhea vhea = tableParser.ReadVhea(binaryReader, vheaInfo);
		TTFHmtx hmtx = tableParser.ReadHmtx(hhea, binaryReader, hmtxInfo); //
		TTFVmtx vmtx = tableParser.ReadVmtx(binaryReader, vmtxInfo); //

		#region temp
		// Scilab source code
		// Print scilab source code
		List<Vector2> controlPoints = new List<Vector2>();

		int resolutionCount = 10;
		int offsetX = 0;

		int[] indexes = new int[26];
		for (int i = 0; i < 26; i++)
			indexes[i] = i + 36;

		List<string> sourceCodeLines = new List<string>();
		for (int j = 0; j < indexes.Length; j++)
		{
			sourceCodeLines.Add("clear;");
			sourceCodeLines.Add("clc;");
			sourceCodeLines.Add("x = [");

			List<Vector2> points = new List<Vector2>(glyf.Glyphs[indexes[j]].XCoordinates.Length);

			for (int i = 0; i < glyf.Glyphs[indexes[j]].XCoordinates.Length; i++)
			{
				bool onCurve = (glyf.Glyphs[indexes[j]].FlagsSimple[i] & (int)TTFGlyf.SIMPLE_FLAGS.ON_CURVE) != 0;
				if (onCurve)
					points.Add(new Vector2(glyf.Glyphs[indexes[j]].XCoordinates[i] + offsetX, glyf.Glyphs[indexes[j]].YCoordinates[i]));
				else
				{
					// Find all control points in succession
					List<Vector2> successionControlPoints = new List<Vector2>();
					for (int k = i; k < glyf.Glyphs[indexes[j]].XCoordinates.Length; k++)
					{
						onCurve = (glyf.Glyphs[indexes[j]].FlagsSimple[k] & (int)TTFGlyf.SIMPLE_FLAGS.ON_CURVE) != 0;
						if (!onCurve)
							successionControlPoints.Add(new Vector2(glyf.Glyphs[indexes[j]].XCoordinates[k], glyf.Glyphs[indexes[j]].YCoordinates[k]));
						else
							break;
					}


					// Find next on curve point based on k (the loop ensures k is on curve due to the break condition)
					// Vector2 nextOnCurve = new Vector2(glyf.Glyphs[indexes[j]].XCoordinates[k], glyf.Glyphs[indexes[j]].YCoordinates[k]);

					// Control points one after another, need to take midpoints
					if (successionControlPoints.Count > 1)
					{
						for (int w = 0; w < successionControlPoints.Count - 1; w++)
						{
							for (int k = 1; k < resolutionCount + 2; k++)
							{
								float t = k * (1f / (resolutionCount + 1));
								int x, y;

								if (w == 0)
								{
									Vector2 nextAverage = (successionControlPoints[w] + successionControlPoints[w + 1]) / 2;
									int prevInd = i - 1;
									x = (int)MathHelper.QBez(glyf.Glyphs[indexes[j]].XCoordinates[i - 1], successionControlPoints[w].X, nextAverage.X, t);
									y = (int)MathHelper.QBez(glyf.Glyphs[indexes[j]].YCoordinates[i - 1], successionControlPoints[w].Y, nextAverage.Y, t); ;
								}
								else if (w == successionControlPoints.Count - 1)
								{
									Vector2 prevAverage = (successionControlPoints[w - 1] + successionControlPoints[w]) / 2;
									int nextInd = i + 1;
									x = (int)MathHelper.QBez(prevAverage.X, successionControlPoints[w].X, glyf.Glyphs[indexes[j]].XCoordinates[i + 1], t);
									y = (int)MathHelper.QBez(prevAverage.Y, successionControlPoints[w].Y, glyf.Glyphs[indexes[j]].YCoordinates[i + 1], t);
								}
								else
								{
									Vector2 prevAverage = (successionControlPoints[w - 1] + successionControlPoints[w]) / 2;
									Vector2 nextAverage = (successionControlPoints[w] + successionControlPoints[w + 1]) / 2;
									x = (int)MathHelper.QBez(prevAverage.X, successionControlPoints[w].X, nextAverage.X, t);
									y = (int)MathHelper.QBez(prevAverage.Y, successionControlPoints[w].Y, nextAverage.Y, t);
								}

								points.Add(new Vector2((x + offsetX), y));
							}
						}
						i += successionControlPoints.Count - 1;
					}
					else
						for (int k = 1; k < resolutionCount + 2; k++)
						{
							if (i == glyf.Glyphs[indexes[j]].XCoordinates.Length - 1)
								break; // NEED TO WRAP

							float t = k * (1f / (resolutionCount + 1));
							int x = (int)MathHelper.QBez(glyf.Glyphs[indexes[j]].XCoordinates[i - 1], glyf.Glyphs[indexes[j]].XCoordinates[i], glyf.Glyphs[indexes[j]].XCoordinates[i + 1], t);
							int y = (int)MathHelper.QBez(glyf.Glyphs[indexes[j]].YCoordinates[i - 1], glyf.Glyphs[indexes[j]].YCoordinates[i], glyf.Glyphs[indexes[j]].YCoordinates[i + 1], t);

							points.Add(new Vector2((x + offsetX), y));
						}
				}
			}

			foreach (Vector2 p in points)
				sourceCodeLines.Add($"{(int)p.X}, {(int)p.Y};");

			sourceCodeLines.Add("];");
			sourceCodeLines.Add("scatter(x(:, 1), x(:, 2));");
			for (int i = 0; i < points.Count; i++)
				sourceCodeLines.Add($"plot([x({i + 1}, 1), x({i + 2}, 1)], [x({i + 1}, 2), x({i + 2}, 2)]);");
			offsetX += 1750;
		}
		sourceCodeLines.Add("xlabel('X');");
		sourceCodeLines.Add("ylabel('Y');");
		sourceCodeLines.Add("xtitle('Scatter Plot', 'X', 'Y');");

		File.WriteAllLines(@"D:\Scilab.txt", sourceCodeLines);
		#endregion

		result = new Font(head, name, loca, glyf, cmap, hhea, vhea, hmtx, vmtx, maxp);
		return true;
	}

	// Parsers
}