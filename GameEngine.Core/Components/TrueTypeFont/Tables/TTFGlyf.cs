namespace GameEngine.Core.Components.TrueTypeFont.Tables;

internal class TTFGlyf
{
	public GlyfData[] Glyphs { get; }

	public TTFGlyf(GlyfData[] glyfs)
	{
		Glyphs = glyfs;
	}

	internal struct GlyfData
	{
		public short NumberOfContours { get; }
		public float XMin { get; }
		public float YMin { get; }
		public float XMax { get; }
		public float YMax { get; }

		// Single glyph
		public ushort[] EndPtsOfContours { get; }
		public ushort instructionLength { get; }
		public byte[] instructions { get; }
		public byte[] flags { get; }
		public short[] XCoordinates { get; }
		public short[] YCoordinates { get; }

		// Compound glyph

		public GlyfData(short numberOfContours, float xMin, float yMin, float xMax, float yMax, ushort[] endPtsOfContours, ushort instructionLength, byte[] instructions, byte[] flags, short[] xCoordinates, short[] yCoordinates)
		{
			NumberOfContours = numberOfContours;
			XMin = xMin;
			YMin = yMin;
			XMax = xMax;
			YMax = yMax;
			EndPtsOfContours = endPtsOfContours;
			this.instructionLength = instructionLength;
			this.instructions = instructions;
			this.flags = flags;
			XCoordinates = xCoordinates;
			YCoordinates = yCoordinates;
		}
	}

	[Flags]
	internal enum SIMPLE_FLAGS
	{
		ON_CURVE = 0b1,
		X_IS_BYTE = 0b10,
		Y_IS_BYTE = 0b100,
		REPEAT = 0b1000,
		X_DELTA = 0b10000,
		Y_DELTA = 0b100000,
	}

	[Flags]
	internal enum COMPOUND_FLAGS
	{

	}
}