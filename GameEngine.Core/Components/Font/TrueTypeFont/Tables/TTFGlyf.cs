namespace GameEngine.Core.Components.Font.TrueTypeFont.Tables;

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
		public ushort InstructionLength { get; }
		public byte[] Instructions { get; }
		public byte[] FlagsSimple { get; }
		public short[] XCoordinates { get; }
		public short[] YCoordinates { get; }

		// Compound glyph
		public ushort FlagsCompound { get; }
		public ushort GlyphIndex { get; }
		public int Argument1 { get; }
		public int Argument2 { get; }
		public int TransformationOption1 { get; }
		public int TransformationOption2 { get; }
		public int TransformationOption3 { get; }
		public int TransformationOption4 { get; }

		public GlyfData(short numberOfContours, float xMin, float yMin, float xMax, float yMax, ushort[] endPtsOfContours, ushort instructionLength, byte[] instructions, byte[] flagsSimple, short[] xCoordinates, short[] yCoordinates, ushort flagsCompound, ushort glyphIndex, int argument1, int argument2, int transformationOption1, int transformationOption2, int transformationOption3, int transformationOption4)
		{
			NumberOfContours = numberOfContours;
			XMin = xMin;
			YMin = yMin;
			XMax = xMax;
			YMax = yMax;
			EndPtsOfContours = endPtsOfContours;
			InstructionLength = instructionLength;
			Instructions = instructions;
			FlagsSimple = flagsSimple;
			XCoordinates = xCoordinates;
			YCoordinates = yCoordinates;

			FlagsCompound = flagsCompound;
			GlyphIndex = glyphIndex;
			Argument1 = argument1;
			Argument2 = argument2;
			TransformationOption1 = transformationOption1;
			TransformationOption2 = transformationOption2;
			TransformationOption3 = transformationOption3;
			TransformationOption4 = transformationOption4;
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