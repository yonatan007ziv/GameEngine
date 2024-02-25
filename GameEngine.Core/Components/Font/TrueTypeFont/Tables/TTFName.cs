namespace GameEngine.Core.Components.Font.TrueTypeFont.Tables;

internal class TTFName
{
	public ushort Format { get; }
	public ushort Count { get; }
	public ushort StringOffset { get; }
	public NameRecordStruct[] NameRecord { get; }
	public string[] Name { get; }

	public TTFName(ushort format, ushort count, ushort stringOffset, NameRecordStruct[] nameRecord, string[] name)
	{
		Format = format;
		Count = count;
		StringOffset = stringOffset;
		NameRecord = nameRecord;
		Name = name;
	}

	internal struct NameRecordStruct
	{
		public ushort platformID;
		public ushort platformSpecificID;
		public ushort languageID;
		public ushort nameID;
		public ushort length;
		public ushort offset;

		public NameRecordStruct(ushort platformID, ushort platformSpecificID, ushort languageID, ushort nameID, ushort length, ushort offset)
		{
			this.platformID = platformID;
			this.platformSpecificID = platformSpecificID;
			this.languageID = languageID;
			this.nameID = nameID;
			this.length = length;
			this.offset = offset;
		}
	}
}