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

    public float ReadFixed32()
    {
        var data = base.ReadBytes(4);
        // Array.Reverse(data);
        return BitConverter.ToSingle(data);
    }

    public sbyte ReadInt8()
    {
        return (sbyte)base.ReadByte();
    }
    public override short ReadInt16()
    {
        var data = base.ReadBytes(2);
        Array.Reverse(data);
        return BitConverter.ToInt16(data);
    }
    public override int ReadInt32()
    {
        var data = base.ReadBytes(4);
        Array.Reverse(data);
        return BitConverter.ToInt32(data);
    }
    public override long ReadInt64()
    {
        var data = base.ReadBytes(8);
        Array.Reverse(data);
        return BitConverter.ToInt64(data);
    }

    public byte ReadUInt8()
    {
        return base.ReadByte();
    }
    public override ushort ReadUInt16()
    {
        var data = base.ReadBytes(2);
        Array.Reverse(data);
        return BitConverter.ToUInt16(data);
    }
    public override uint ReadUInt32()
    {
        var data = base.ReadBytes(4);
        Array.Reverse(data);
        return BitConverter.ToUInt32(data);
    }
    public override ulong ReadUInt64()
    {
        var data = base.ReadBytes(8);
        Array.Reverse(data);
        return BitConverter.ToUInt64(data);
    }
}