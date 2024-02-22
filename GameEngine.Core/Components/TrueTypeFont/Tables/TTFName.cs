namespace GameEngine.Core.Components.TrueTypeFont.Tables;

internal class TTFName
{
	public ushort format { get; }
	public ushort count { get; }
	public ushort stringOffset { get; }
	public NameRecord[] nameRecord { get; }
	public string[] name { get; }

	public TTFName(ushort format, ushort count, ushort stringOffset, NameRecord[] nameRecord, string[] name)
	{
		this.format = format;
		this.count = count;
		this.stringOffset = stringOffset;
		this.nameRecord = nameRecord;
		this.name = name;
	}

	internal struct NameRecord
	{
		public ushort platformID;
		public ushort platformSpecificID;
		public ushort languageID;
		public ushort nameID;
		public ushort length;
		public ushort offset;

		public NameRecord(ushort platformID, ushort platformSpecificID, ushort languageID, ushort nameID, ushort length, ushort offset)
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