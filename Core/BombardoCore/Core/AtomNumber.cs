using System;
using System.Runtime.InteropServices;

namespace Bombardo.Core
{
    [StructLayout(LayoutKind.Explicit)]
    public struct AtomNumber
    {
        [FieldOffset(0)]           public int     type;
        [FieldOffset(sizeof(int))] public byte    val_uint8;
        [FieldOffset(sizeof(int))] public sbyte   val_sint8;
        [FieldOffset(sizeof(int))] public ushort  val_uint16;
        [FieldOffset(sizeof(int))] public short   val_sint16;
        [FieldOffset(sizeof(int))] public char    val_char;
        [FieldOffset(sizeof(int))] public uint    val_uint32;
        [FieldOffset(sizeof(int))] public int     val_sint32;
        [FieldOffset(sizeof(int))] public ulong   val_uint64;
        [FieldOffset(sizeof(int))] public long    val_sint64;
        [FieldOffset(sizeof(int))] public float   val_single;
        [FieldOffset(sizeof(int))] public double  val_double;
        [FieldOffset(sizeof(int))] public decimal val_decimal;

        public override string ToString()
        {
            switch (type)
            {
                case AtomNumberType.UINT_8:
                    return val_uint8.ToString();
                case AtomNumberType.SINT_8:
                    return val_uint8.ToString();
                case AtomNumberType.UINT16:
                    return val_uint16.ToString();
                case AtomNumberType.SINT16:
                    return val_uint16.ToString();
                case AtomNumberType._CHAR_:
                    return $"'{val_char}'";
                case AtomNumberType.UINT32:
                    return val_uint32.ToString();
                case AtomNumberType.SINT32:
                    return val_uint32.ToString();
                case AtomNumberType.UINT64:
                    return val_uint64.ToString();
                case AtomNumberType.SINT64:
                    return val_sint64.ToString();
                case AtomNumberType.SINGLE:
                    return $"{val_single:0.00}";
                case AtomNumberType.DOUBLE:
                    return $"{val_double:0.00000000}";
                case AtomNumberType.DECIMAL:
                    return $"{val_decimal:0.00000000000000000000000000000000}";
            }

            return "NaN";
        }

        private Exception TypeMismatch(int from, int to)
        {
            return new Exception(
                $"Number type mismatch! Trying cast {AtomNumberType.ToString(from)} to {AtomNumberType.ToString(to)}");
        }

        public byte ToUByte()
        {
            switch (type)
            {
                case AtomNumberType.UINT_8:  return (byte) val_uint8;
                case AtomNumberType.SINT_8:  return (byte) val_sint8;
                case AtomNumberType.UINT16:  return (byte) val_uint16;
                case AtomNumberType.SINT16:  return (byte) val_sint16;
                case AtomNumberType._CHAR_:  return (byte) val_char;
                case AtomNumberType.UINT32:  return (byte) val_uint32;
                case AtomNumberType.SINT32:  return (byte) val_sint32;
                case AtomNumberType.UINT64:  return (byte) val_uint64;
                case AtomNumberType.SINT64:  return (byte) val_sint64;
                case AtomNumberType.SINGLE:  return (byte) val_single;
                case AtomNumberType.DOUBLE:  return (byte) val_double;
                case AtomNumberType.DECIMAL: return (byte) val_decimal;
            }

            throw TypeMismatch(type, AtomNumberType.UINT_8);
        }

        public sbyte ToSByte()
        {
            switch (type)
            {
                case AtomNumberType.UINT_8:  return (sbyte) val_uint8;
                case AtomNumberType.SINT_8:  return (sbyte) val_sint8;
                case AtomNumberType.UINT16:  return (sbyte) val_uint16;
                case AtomNumberType.SINT16:  return (sbyte) val_sint16;
                case AtomNumberType._CHAR_:  return (sbyte) val_char;
                case AtomNumberType.UINT32:  return (sbyte) val_uint32;
                case AtomNumberType.SINT32:  return (sbyte) val_sint32;
                case AtomNumberType.UINT64:  return (sbyte) val_uint64;
                case AtomNumberType.SINT64:  return (sbyte) val_sint64;
                case AtomNumberType.SINGLE:  return (sbyte) val_single;
                case AtomNumberType.DOUBLE:  return (sbyte) val_double;
                case AtomNumberType.DECIMAL: return (sbyte) val_decimal;
            }

            throw TypeMismatch(type, AtomNumberType.SINT_8);
        }

        public ushort ToUShort()
        {
            switch (type)
            {
                case AtomNumberType.UINT_8:  return (ushort) val_uint8;
                case AtomNumberType.SINT_8:  return (ushort) val_sint8;
                case AtomNumberType.UINT16:  return (ushort) val_uint16;
                case AtomNumberType.SINT16:  return (ushort) val_sint16;
                case AtomNumberType._CHAR_:  return (ushort) val_char;
                case AtomNumberType.UINT32:  return (ushort) val_uint32;
                case AtomNumberType.SINT32:  return (ushort) val_sint32;
                case AtomNumberType.UINT64:  return (ushort) val_uint64;
                case AtomNumberType.SINT64:  return (ushort) val_sint64;
                case AtomNumberType.SINGLE:  return (ushort) val_single;
                case AtomNumberType.DOUBLE:  return (ushort) val_double;
                case AtomNumberType.DECIMAL: return (ushort) val_decimal;
            }

            throw TypeMismatch(type, AtomNumberType.UINT16);
        }

        public short ToSShort()
        {
            switch (type)
            {
                case AtomNumberType.UINT_8:  return (short) val_uint8;
                case AtomNumberType.SINT_8:  return (short) val_sint8;
                case AtomNumberType.UINT16:  return (short) val_uint16;
                case AtomNumberType.SINT16:  return (short) val_sint16;
                case AtomNumberType._CHAR_:  return (short) val_char;
                case AtomNumberType.UINT32:  return (short) val_uint32;
                case AtomNumberType.SINT32:  return (short) val_sint32;
                case AtomNumberType.UINT64:  return (short) val_uint64;
                case AtomNumberType.SINT64:  return (short) val_sint64;
                case AtomNumberType.SINGLE:  return (short) val_single;
                case AtomNumberType.DOUBLE:  return (short) val_double;
                case AtomNumberType.DECIMAL: return (short) val_decimal;
            }

            throw TypeMismatch(type, AtomNumberType.SINT16);
        }

        public char ToChar()
        {
            switch (type)
            {
                case AtomNumberType.UINT_8:  return (char) val_uint8;
                case AtomNumberType.SINT_8:  return (char) val_sint8;
                case AtomNumberType.UINT16:  return (char) val_uint16;
                case AtomNumberType.SINT16:  return (char) val_sint16;
                case AtomNumberType._CHAR_:  return (char) val_char;
                case AtomNumberType.UINT32:  return (char) val_uint32;
                case AtomNumberType.SINT32:  return (char) val_sint32;
                case AtomNumberType.UINT64:  return (char) val_uint64;
                case AtomNumberType.SINT64:  return (char) val_sint64;
                case AtomNumberType.SINGLE:  return (char) val_single;
                case AtomNumberType.DOUBLE:  return (char) val_double;
                case AtomNumberType.DECIMAL: return (char) val_double;
            }

            throw TypeMismatch(type, AtomNumberType.DOUBLE);
        }

        public uint ToUInt()
        {
            switch (type)
            {
                case AtomNumberType.UINT_8:  return (uint) val_uint8;
                case AtomNumberType.SINT_8:  return (uint) val_sint8;
                case AtomNumberType.UINT16:  return (uint) val_uint16;
                case AtomNumberType.SINT16:  return (uint) val_sint16;
                case AtomNumberType._CHAR_:  return (uint) val_char;
                case AtomNumberType.UINT32:  return (uint) val_uint32;
                case AtomNumberType.SINT32:  return (uint) val_sint32;
                case AtomNumberType.UINT64:  return (uint) val_uint64;
                case AtomNumberType.SINT64:  return (uint) val_sint64;
                case AtomNumberType.SINGLE:  return (uint) val_single;
                case AtomNumberType.DOUBLE:  return (uint) val_double;
                case AtomNumberType.DECIMAL: return (uint) val_decimal;
            }

            throw TypeMismatch(type, AtomNumberType.UINT32);
        }

        public int ToSInt()
        {
            switch (type)
            {
                case AtomNumberType.UINT_8:  return (int) val_uint8;
                case AtomNumberType.SINT_8:  return (int) val_sint8;
                case AtomNumberType.UINT16:  return (int) val_uint16;
                case AtomNumberType.SINT16:  return (int) val_sint16;
                case AtomNumberType._CHAR_:  return (int) val_char;
                case AtomNumberType.UINT32:  return (int) val_uint32;
                case AtomNumberType.SINT32:  return (int) val_sint32;
                case AtomNumberType.UINT64:  return (int) val_uint64;
                case AtomNumberType.SINT64:  return (int) val_sint64;
                case AtomNumberType.SINGLE:  return (int) val_single;
                case AtomNumberType.DOUBLE:  return (int) val_double;
                case AtomNumberType.DECIMAL: return (int) val_decimal;
            }

            throw TypeMismatch(type, AtomNumberType.SINT32);
        }

        public ulong ToULong()
        {
            switch (type)
            {
                case AtomNumberType.UINT_8:  return (ulong) val_uint8;
                case AtomNumberType.SINT_8:  return (ulong) val_sint8;
                case AtomNumberType.UINT16:  return (ulong) val_uint16;
                case AtomNumberType.SINT16:  return (ulong) val_sint16;
                case AtomNumberType._CHAR_:  return (ulong) val_char;
                case AtomNumberType.UINT32:  return (ulong) val_uint32;
                case AtomNumberType.SINT32:  return (ulong) val_sint32;
                case AtomNumberType.UINT64:  return (ulong) val_uint64;
                case AtomNumberType.SINT64:  return (ulong) val_sint64;
                case AtomNumberType.SINGLE:  return (ulong) val_single;
                case AtomNumberType.DOUBLE:  return (ulong) val_double;
                case AtomNumberType.DECIMAL: return (ulong) val_decimal;
            }

            throw TypeMismatch(type, AtomNumberType.UINT64);
        }

        public long ToSLong()
        {
            switch (type)
            {
                case AtomNumberType.UINT_8:  return (long) val_uint8;
                case AtomNumberType.SINT_8:  return (long) val_sint8;
                case AtomNumberType.UINT16:  return (long) val_uint16;
                case AtomNumberType.SINT16:  return (long) val_sint16;
                case AtomNumberType._CHAR_:  return (long) val_char;
                case AtomNumberType.UINT32:  return (long) val_uint32;
                case AtomNumberType.SINT32:  return (long) val_sint32;
                case AtomNumberType.UINT64:  return (long) val_uint64;
                case AtomNumberType.SINT64:  return (long) val_sint64;
                case AtomNumberType.SINGLE:  return (long) val_single;
                case AtomNumberType.DOUBLE:  return (long) val_double;
                case AtomNumberType.DECIMAL: return (long) val_decimal;
            }

            throw TypeMismatch(type, AtomNumberType.SINT64);
        }

        public float ToSingle()
        {
            switch (type)
            {
                case AtomNumberType.UINT_8:  return (float) val_uint8;
                case AtomNumberType.SINT_8:  return (float) val_sint8;
                case AtomNumberType.UINT16:  return (float) val_uint16;
                case AtomNumberType.SINT16:  return (float) val_sint16;
                case AtomNumberType._CHAR_:  return (float) val_char;
                case AtomNumberType.UINT32:  return (float) val_uint32;
                case AtomNumberType.SINT32:  return (float) val_sint32;
                case AtomNumberType.UINT64:  return (float) val_uint64;
                case AtomNumberType.SINT64:  return (float) val_sint64;
                case AtomNumberType.SINGLE:  return (float) val_single;
                case AtomNumberType.DOUBLE:  return (float) val_double;
                case AtomNumberType.DECIMAL: return (float) val_decimal;
            }

            throw TypeMismatch(type, AtomNumberType.SINGLE);
        }

        public double ToDouble()
        {
            switch (type)
            {
                case AtomNumberType.UINT_8:  return (double) val_uint8;
                case AtomNumberType.SINT_8:  return (double) val_sint8;
                case AtomNumberType.UINT16:  return (double) val_uint16;
                case AtomNumberType.SINT16:  return (double) val_sint16;
                case AtomNumberType._CHAR_:  return (double) val_char;
                case AtomNumberType.UINT32:  return (double) val_uint32;
                case AtomNumberType.SINT32:  return (double) val_sint32;
                case AtomNumberType.UINT64:  return (double) val_uint64;
                case AtomNumberType.SINT64:  return (double) val_sint64;
                case AtomNumberType.SINGLE:  return (double) val_single;
                case AtomNumberType.DOUBLE:  return (double) val_double;
                case AtomNumberType.DECIMAL: return (double) val_decimal;
            }

            throw TypeMismatch(type, AtomNumberType.DOUBLE);
        }

        public decimal ToDecimal()
        {
            switch (type)
            {
                case AtomNumberType.UINT_8:  return (decimal) val_uint8;
                case AtomNumberType.SINT_8:  return (decimal) val_sint8;
                case AtomNumberType.UINT16:  return (decimal) val_uint16;
                case AtomNumberType.SINT16:  return (decimal) val_sint16;
                case AtomNumberType._CHAR_:  return (decimal) val_char;
                case AtomNumberType.UINT32:  return (decimal) val_uint32;
                case AtomNumberType.SINT32:  return (decimal) val_sint32;
                case AtomNumberType.UINT64:  return (decimal) val_uint64;
                case AtomNumberType.SINT64:  return (decimal) val_sint64;
                case AtomNumberType.SINGLE:  return (decimal) val_single;
                case AtomNumberType.DOUBLE:  return (decimal) val_double;
                case AtomNumberType.DECIMAL: return (decimal) val_decimal;
            }

            throw TypeMismatch(type, AtomNumberType.DECIMAL);
        }
    }
}