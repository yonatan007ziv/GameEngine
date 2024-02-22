﻿namespace GameEngine.Core.Components;

internal class BigEndianBinaryReader : BinaryReader
{
	public BigEndianBinaryReader(Stream stream)
		: base(stream)
	{

	}

	public override int ReadInt32()
	{
		var data = base.ReadBytes(4);
		Array.Reverse(data);
		return BitConverter.ToInt32(data, 0);
	}

	public Int16 ReadInt16()
	{
		var data = base.ReadBytes(2);
		Array.Reverse(data);
		return BitConverter.ToInt16(data, 0);
	}

	public Int64 ReadInt64()
	{
		var data = base.ReadBytes(8);
		Array.Reverse(data);
		return BitConverter.ToInt64(data, 0);
	}

	public UInt16 ReadUInt16()
	{
		var data = base.ReadBytes(2);
		Array.Reverse(data);
		return BitConverter.ToUInt16(data, 0);
	}

	public UInt32 ReadUInt32()
	{
		var data = base.ReadBytes(4);
		Array.Reverse(data);
		return BitConverter.ToUInt32(data, 0);
	}
}