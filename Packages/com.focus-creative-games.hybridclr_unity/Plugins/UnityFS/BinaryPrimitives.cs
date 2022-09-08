//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace UnityFS
//{

//    public static class BinaryPrimitives
//    {
//        public static sbyte ReverseEndianness(sbyte value)
//        {
//            return value;
//        }

//        public static short ReverseEndianness(short value)
//        {
//            return (short)(((value & 0xFF) << 8) | ((value & 0xFF00) >> 8));
//        }


//        public static int ReverseEndianness(int value)
//        {
//            return (int)ReverseEndianness((uint)value);
//        }

//        public static long ReverseEndianness(long value)
//        {
//            return (long)ReverseEndianness((ulong)value);
//        }


//        public static byte ReverseEndianness(byte value)
//        {
//            return value;
//        }

//        public static ushort ReverseEndianness(ushort value)
//        {
//            return (ushort)((value >> 8) + (value << 8));
//        }

//        public static uint ReverseEndianness(uint value)
//        {
//            uint num = value & 0xFF00FFu;
//            uint num2 = value & 0xFF00FF00u;
//            return ((num >> 8) | (num << 24)) + ((num2 << 8) | (num2 >> 24));
//        }

//        public static ulong ReverseEndianness(ulong value)
//        {
//            return ((ulong)ReverseEndianness((uint)value) << 32) + ReverseEndianness((uint)(value >> 32));
//        }

//        public static short ReadInt16BigEndian(byte[] source)
//        {
//            short num = MemoryMarshal.Read<short>(source);
//            if (BitConverter.IsLittleEndian)
//            {
//                num = ReverseEndianness(num);
//            }
//            return num;
//        }


//        public static int ReadInt32BigEndian(byte[] source)
//        {
//            int num = MemoryMarshal.Read<int>(source);
//            if (BitConverter.IsLittleEndian)
//            {
//                num = ReverseEndianness(num);
//            }
//            return num;
//        }


//        public static long ReadInt64BigEndian(byte[] source)
//        {
//            long num = MemoryMarshal.Read<long>(source);
//            if (BitConverter.IsLittleEndian)
//            {
//                num = ReverseEndianness(num);
//            }
//            return num;
//        }



//        public static ushort ReadUInt16BigEndian(byte[] source)
//        {
//            ushort num = MemoryMarshal.Read<ushort>(source);
//            if (BitConverter.IsLittleEndian)
//            {
//                num = ReverseEndianness(num);
//            }
//            return num;
//        }



//        public static uint ReadUInt32BigEndian(byte[] source)
//        {
//            uint num = MemoryMarshal.Read<uint>(source);
//            if (BitConverter.IsLittleEndian)
//            {
//                num = ReverseEndianness(num);
//            }
//            return num;
//        }



//        public static ulong ReadUInt64BigEndian(byte[] source)
//        {
//            ulong num = MemoryMarshal.Read<ulong>(source);
//            if (BitConverter.IsLittleEndian)
//            {
//                num = ReverseEndianness(num);
//            }
//            return num;
//        }


//        public static bool TryReadInt16BigEndian(byte[] source, out short value)
//        {
//            bool result = MemoryMarshal.TryRead<short>(source, out value);
//            if (BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return result;
//        }


//        public static bool TryReadInt32BigEndian(byte[] source, out int value)
//        {
//            bool result = MemoryMarshal.TryRead<int>(source, out value);
//            if (BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return result;
//        }


//        public static bool TryReadInt64BigEndian(byte[] source, out long value)
//        {
//            bool result = MemoryMarshal.TryRead<long>(source, out value);
//            if (BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return result;
//        }



//        public static bool TryReadUInt16BigEndian(byte[] source, out ushort value)
//        {
//            bool result = MemoryMarshal.TryRead<ushort>(source, out value);
//            if (BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return result;
//        }



//        public static bool TryReadUInt32BigEndian(byte[] source, out uint value)
//        {
//            bool result = MemoryMarshal.TryRead<uint>(source, out value);
//            if (BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return result;
//        }



//        public static bool TryReadUInt64BigEndian(byte[] source, out ulong value)
//        {
//            bool result = MemoryMarshal.TryRead<ulong>(source, out value);
//            if (BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return result;
//        }


//        public static short ReadInt16LittleEndian(byte[] source)
//        {
//            short num = MemoryMarshal.Read<short>(source);
//            if (!BitConverter.IsLittleEndian)
//            {
//                num = ReverseEndianness(num);
//            }
//            return num;
//        }


//        public static int ReadInt32LittleEndian(byte[] source)
//        {
//            int num = MemoryMarshal.Read<int>(source);
//            if (!BitConverter.IsLittleEndian)
//            {
//                num = ReverseEndianness(num);
//            }
//            return num;
//        }


//        public static long ReadInt64LittleEndian(byte[] source)
//        {
//            long num = MemoryMarshal.Read<long>(source);
//            if (!BitConverter.IsLittleEndian)
//            {
//                num = ReverseEndianness(num);
//            }
//            return num;
//        }



//        public static ushort ReadUInt16LittleEndian(byte[] source)
//        {
//            ushort num = MemoryMarshal.Read<ushort>(source);
//            if (!BitConverter.IsLittleEndian)
//            {
//                num = ReverseEndianness(num);
//            }
//            return num;
//        }



//        public static uint ReadUInt32LittleEndian(byte[] source)
//        {
//            uint num = MemoryMarshal.Read<uint>(source);
//            if (!BitConverter.IsLittleEndian)
//            {
//                num = ReverseEndianness(num);
//            }
//            return num;
//        }



//        public static ulong ReadUInt64LittleEndian(byte[] source)
//        {
//            ulong num = MemoryMarshal.Read<ulong>(source);
//            if (!BitConverter.IsLittleEndian)
//            {
//                num = ReverseEndianness(num);
//            }
//            return num;
//        }


//        public static bool TryReadInt16LittleEndian(byte[] source, out short value)
//        {
//            bool result = MemoryMarshal.TryRead<short>(source, out value);
//            if (!BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return result;
//        }


//        public static bool TryReadInt32LittleEndian(byte[] source, out int value)
//        {
//            bool result = MemoryMarshal.TryRead<int>(source, out value);
//            if (!BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return result;
//        }


//        public static bool TryReadInt64LittleEndian(byte[] source, out long value)
//        {
//            bool result = MemoryMarshal.TryRead<long>(source, out value);
//            if (!BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return result;
//        }



//        public static bool TryReadUInt16LittleEndian(byte[] source, out ushort value)
//        {
//            bool result = MemoryMarshal.TryRead<ushort>(source, out value);
//            if (!BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return result;
//        }



//        public static bool TryReadUInt32LittleEndian(byte[] source, out uint value)
//        {
//            bool result = MemoryMarshal.TryRead<uint>(source, out value);
//            if (!BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return result;
//        }



//        public static bool TryReadUInt64LittleEndian(byte[] source, out ulong value)
//        {
//            bool result = MemoryMarshal.TryRead<ulong>(source, out value);
//            if (!BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return result;
//        }


//        public static void WriteInt16BigEndian(Span<byte> destination, short value)
//        {
//            if (BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            MemoryMarshal.Write(destination, ref value);
//        }


//        public static void WriteInt32BigEndian(Span<byte> destination, int value)
//        {
//            if (BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            MemoryMarshal.Write(destination, ref value);
//        }


//        public static void WriteInt64BigEndian(Span<byte> destination, long value)
//        {
//            if (BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            MemoryMarshal.Write(destination, ref value);
//        }



//        public static void WriteUInt16BigEndian(Span<byte> destination, ushort value)
//        {
//            if (BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            MemoryMarshal.Write(destination, ref value);
//        }



//        public static void WriteUInt32BigEndian(Span<byte> destination, uint value)
//        {
//            if (BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            MemoryMarshal.Write(destination, ref value);
//        }



//        public static void WriteUInt64BigEndian(Span<byte> destination, ulong value)
//        {
//            if (BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            MemoryMarshal.Write(destination, ref value);
//        }


//        public static bool TryWriteInt16BigEndian(Span<byte> destination, short value)
//        {
//            if (BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return MemoryMarshal.TryWrite(destination, ref value);
//        }


//        public static bool TryWriteInt32BigEndian(Span<byte> destination, int value)
//        {
//            if (BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return MemoryMarshal.TryWrite(destination, ref value);
//        }


//        public static bool TryWriteInt64BigEndian(Span<byte> destination, long value)
//        {
//            if (BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return MemoryMarshal.TryWrite(destination, ref value);
//        }



//        public static bool TryWriteUInt16BigEndian(Span<byte> destination, ushort value)
//        {
//            if (BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return MemoryMarshal.TryWrite(destination, ref value);
//        }



//        public static bool TryWriteUInt32BigEndian(Span<byte> destination, uint value)
//        {
//            if (BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return MemoryMarshal.TryWrite(destination, ref value);
//        }



//        public static bool TryWriteUInt64BigEndian(Span<byte> destination, ulong value)
//        {
//            if (BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return MemoryMarshal.TryWrite(destination, ref value);
//        }


//        public static void WriteInt16LittleEndian(Span<byte> destination, short value)
//        {
//            if (!BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            MemoryMarshal.Write(destination, ref value);
//        }


//        public static void WriteInt32LittleEndian(Span<byte> destination, int value)
//        {
//            if (!BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            MemoryMarshal.Write(destination, ref value);
//        }


//        public static void WriteInt64LittleEndian(Span<byte> destination, long value)
//        {
//            if (!BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            MemoryMarshal.Write(destination, ref value);
//        }



//        public static void WriteUInt16LittleEndian(Span<byte> destination, ushort value)
//        {
//            if (!BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            MemoryMarshal.Write(destination, ref value);
//        }



//        public static void WriteUInt32LittleEndian(Span<byte> destination, uint value)
//        {
//            if (!BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            MemoryMarshal.Write(destination, ref value);
//        }



//        public static void WriteUInt64LittleEndian(Span<byte> destination, ulong value)
//        {
//            if (!BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            MemoryMarshal.Write(destination, ref value);
//        }


//        public static bool TryWriteInt16LittleEndian(Span<byte> destination, short value)
//        {
//            if (!BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return MemoryMarshal.TryWrite(destination, ref value);
//        }


//        public static bool TryWriteInt32LittleEndian(Span<byte> destination, int value)
//        {
//            if (!BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return MemoryMarshal.TryWrite(destination, ref value);
//        }


//        public static bool TryWriteInt64LittleEndian(Span<byte> destination, long value)
//        {
//            if (!BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return MemoryMarshal.TryWrite(destination, ref value);
//        }



//        public static bool TryWriteUInt16LittleEndian(Span<byte> destination, ushort value)
//        {
//            if (!BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return MemoryMarshal.TryWrite(destination, ref value);
//        }



//        public static bool TryWriteUInt32LittleEndian(Span<byte> destination, uint value)
//        {
//            if (!BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return MemoryMarshal.TryWrite(destination, ref value);
//        }



//        public static bool TryWriteUInt64LittleEndian(Span<byte> destination, ulong value)
//        {
//            if (!BitConverter.IsLittleEndian)
//            {
//                value = ReverseEndianness(value);
//            }
//            return MemoryMarshal.TryWrite(destination, ref value);
//        }
//    }

//}
