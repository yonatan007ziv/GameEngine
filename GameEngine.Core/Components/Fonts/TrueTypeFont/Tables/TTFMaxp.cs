namespace GameEngine.Core.Components.Fonts.TrueTypeFont.Tables;

internal class TTFMaxp
{
	public float Version { get; }
	public ushort NumGlyphs { get; }
	public ushort MaxPoints { get; }
	public ushort MaxContours { get; }
	public ushort MaxComponentPoints { get; }
	public ushort MaxComponentContours { get; }
	public ushort MaxZones { get; }
	public ushort MaxTwilightPoints { get; }
	public ushort MaxStorage { get; }
	public ushort MaxFunctionDefs { get; }
	public ushort MaxInstructionDefs { get; }
	public ushort MaxStackElements { get; }
	public ushort MaxSizeOfInstructions { get; }
	public ushort MaxComponentElements { get; }
	public ushort MaxComponentDepth { get; }

	public TTFMaxp(float version, ushort numGlyphs, ushort maxPoints, ushort maxContours, ushort maxComponentPoints, ushort maxComponentContours, ushort maxZones, ushort maxTwilightPoints, ushort maxStorage, ushort maxFunctionDefs, ushort maxInstructionDefs, ushort maxStackElements, ushort maxSizeOfInstructions, ushort maxComponentElements, ushort maxComponentDepth)
	{
		Version = version;
		NumGlyphs = numGlyphs;
		MaxPoints = maxPoints;
		MaxContours = maxContours;
		MaxComponentPoints = maxComponentPoints;
		MaxComponentContours = maxComponentContours;
		MaxZones = maxZones;
		MaxTwilightPoints = maxTwilightPoints;
		MaxStorage = maxStorage;
		MaxFunctionDefs = maxFunctionDefs;
		MaxInstructionDefs = maxInstructionDefs;
		MaxStackElements = maxStackElements;
		MaxSizeOfInstructions = maxSizeOfInstructions;
		MaxComponentElements = maxComponentElements;
		MaxComponentDepth = maxComponentDepth;
	}
}