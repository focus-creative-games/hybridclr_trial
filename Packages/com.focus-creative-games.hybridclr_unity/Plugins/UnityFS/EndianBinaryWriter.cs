using System;
using System.Buffers.Binary;
using System.IO;

namespace UnityFS
{
    public class EndianBinaryWriter : BinaryWriter
    {
        private readonly byte[] buffer;

        public EndianType Endian;

        public EndianBinaryWriter(Stream stream, EndianType endian = EndianType.BigEndian) : base(stream)
        {
            Endian = endian;
            buffer = new byte[8];
        }

        public long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

        public long Length => BaseStream.Length;

        public override void Write(short x)
        {
            Write((ushort)x);
        }

        public override void Write(ushort x)
        {
            if (Endian == EndianType.BigEndian)
            {
                BinaryPrimitives.WriteUInt16BigEndian(buffer, x);
                Write(buffer, 0, 2);
                return;
            }
            base.Write(x);
        }

        public override void Write(int x)
        {
            Write((uint)x);
        }

        public override void Write(uint x)
        {
            if (Endian == EndianType.BigEndian)
            {
                BinaryPrimitives.WriteUInt32BigEndian(buffer, x);
                Write(buffer, 0, 4);
                return;
            }
            base.Write(x);
        }

        public override void Write(long x)
        {
            Write((ulong)x);
        }

        public override void Write(ulong x)
        {
            if (Endian == EndianType.BigEndian)
            {
                BinaryPrimitives.WriteUInt64BigEndian(buffer, x);
                Write(buffer, 0, 8);
                return;
            }
            base.Write(x);
        }

        public override void Write(float x)
        {
            if (Endian == EndianType.BigEndian)
            {
                var buf = BitConverter.GetBytes(x);
                Array.Reverse(buf, 0, 4);
                Write(buf, 0, 4);
                return;
            }
            base.Write(x);
        }

        public override void Write(double x)
        {
            if (Endian == EndianType.BigEndian)
            {
                var buf = BitConverter.GetBytes(x);
                Array.Reverse(buf, 0, 8);
                Write(buf, 0, 8);
                return;
            }
            base.Write(x);
        }
    }
}
