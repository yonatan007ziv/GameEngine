namespace GameEngine.Core.Components.TrueTypeFont.Tables;

internal class TTFCmap
{
	internal struct Subtable
	{
		public ushort platformID;
		public ushort platformSpecificID;
		public uint offset;

		public Subtable(ushort platformID, ushort platformSpecificID, uint offset)
		{
			this.platformID = platformID;
			this.platformSpecificID = platformSpecificID;
			this.offset = offset;
		}
	}
}