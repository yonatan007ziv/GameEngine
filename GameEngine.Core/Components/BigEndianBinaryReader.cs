namespace GameEngine.Core.Components;

internal class BigEndianBinaryReader : BinaryReader
{
	public BinaryReader Base => this;

	public BigEndianBinaryReader(Stream stream)
		: base(stream)
	{

	}


	public byte[] ReadBytesBigEndian(int bytes)
	{
		return base.ReadBytes(bytes).ToArray();
	}

	public Single ReadFixed32()
	{
		var data = base.ReadBytes(4);
		// Array.Reverse(data);
		return BitConverter.ToSingle(data);
	}

	public SByte ReadInt8()
	{
		return (sbyte)base.ReadByte();
	}
	public override Int16 ReadInt16()
	{
		var data = base.ReadBytes(2);
		Array.Reverse(data);
		return BitConverter.ToInt16(data);
	}
	public override Int32 ReadInt32()
	{
		var data = base.ReadBytes(4);
		Array.Reverse(data);
		return BitConverter.ToInt32(data);
	}
	public override Int64 ReadInt64()
	{
		var data = base.ReadBytes(8);
		Array.Reverse(data);
		return BitConverter.ToInt64(data);
	}

	public Byte ReadUInt8()
	{
		return base.ReadByte();
	}
	public override UInt16 ReadUInt16()
	{
		var data = base.ReadBytes(2);
		Array.Reverse(data);
		return BitConverter.ToUInt16(data);
	}
	public override UInt32 ReadUInt32()
	{
		var data = base.ReadBytes(4);
		Array.Reverse(data);
		return BitConverter.ToUInt32(data);
	}
	public override UInt64 ReadUInt64()
	{
		var data = base.ReadBytes(8);
		Array.Reverse(data);
		return BitConverter.ToUInt64(data);
	}
}