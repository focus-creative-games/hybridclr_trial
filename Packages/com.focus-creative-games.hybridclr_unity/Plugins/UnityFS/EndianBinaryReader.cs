using System;
using System.Buffers.Binary;
using System.IO;

namespace UnityFS
{
    public class EndianBinaryReader : BinaryReader
    {
        private readonly byte[] buffer;

        public EndianType Endian;

        public EndianBinaryReader(Stream stream, EndianType endian = EndianType.BigEndian) : base(stream)
        {
            Endian = endian;
            buffer = new byte[8];
        }

        public long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

        public override short ReadInt16()
        {
            return (short)ReadUInt16();
        }

        public override ushort ReadUInt16()
        {
            if (Endian == EndianType.BigEndian)
            {
                Read(buffer, 0, 2);
                return BinaryPrimitives.ReadUInt16BigEndian(buffer);
            }
            return base.ReadUInt16();
        }

        public override int ReadInt32()
        {
            return (int)ReadUInt32();
        }

        public override uint ReadUInt32()
        {
            if (Endian == EndianType.BigEndian)
            {
                Read(buffer, 0, 4);
                return BinaryPrimitives.ReadUInt32BigEndian(buffer);
            }
            return base.ReadUInt32();
        }

        public override long ReadInt64()
        {
            return (long)ReadUInt64();
        }

        public override ulong ReadUInt64()
        {
            if (Endian == EndianType.BigEndian)
            {
                Read(buffer, 0, 8);

                return BinaryPrimitives.ReadUInt64BigEndian(buffer);
            }
            return base.ReadUInt64();
        }

        public override float ReadSingle()
        {
            if (Endian == EndianType.BigEndian)
            {
                Read(buffer, 0, 4);
                Array.Reverse(buffer, 0, 4);
                return BitConverter.ToSingle(buffer, 0);
            }
            return base.ReadSingle();
        }

        public override double ReadDouble()
        {
            if (Endian == EndianType.BigEndian)
            {
                Read(buffer, 0, 8);
                Array.Reverse(buffer);
                return BitConverter.ToDouble(buffer, 0);
            }
            return base.ReadDouble();
        }
    }
}
