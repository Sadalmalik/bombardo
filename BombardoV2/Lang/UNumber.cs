using System;

namespace Bombardo.V2
{
    internal class UNumber
    {
        public const int NaN = int.MaxValue;    //  if one of two numbers is NaN - all is NaN

        public const int UBYTE = 0;
        public const int SBYTE = 1;
        public const int USHORT = 2;
        public const int SSHORT = 3;
        public const int CHAR = 4;
        public const int UINT = 5;
        public const int SINT = 6;
        public const int ULONG = 7;
        public const int SLONG = 8;
        public const int FLOAT = 9;
        public const int DOUBLE = 10;

        public const int UINT_8 = 0;
        public const int SINT_8 = 1;
        public const int UINT16 = 2;
        public const int SINT16 = 3;
        public const int _CHAR_ = 4;
        public const int UINT32 = 5;
        public const int SINT32 = 6;
        public const int UINT64 = 7;
        public const int SINT64 = 8;
        public const int FLO32 = 9;
        public const int FLO64 = 10;

        public static int NumberType(object n)
        {
            if (n is byte) return UBYTE;
            if (n is sbyte) return SBYTE;
            if (n is ushort) return USHORT;
            if (n is short) return SSHORT;
            if (n is char) return CHAR;
            if (n is uint) return UINT;
            if (n is int) return SINT;
            if (n is ulong) return ULONG;
            if (n is long) return SLONG;
            if (n is float) return FLOAT;
            if (n is double) return DOUBLE;
            return NaN;
        }

        public static bool IsNumber(object n)
        {
            return NaN != NumberType(n);
        }

        public static int MaxNumberType(object a, object b)
        {
            return Math.Max(NumberType(a), NumberType(b));
        }

        #region Math operators

        public static object Sum(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8: return Convert.ToByte(a) + Convert.ToByte(b);
                case SINT_8: return Convert.ToSByte(a) + Convert.ToSByte(b);
                case UINT16: return Convert.ToUInt16(a) + Convert.ToUInt16(b);
                case SINT16: return Convert.ToInt16(a) + Convert.ToInt16(b);
                case _CHAR_: return Convert.ToChar(a) + Convert.ToChar(b);
                case UINT32: return Convert.ToUInt32(a) + Convert.ToUInt32(b);
                case SINT32: return Convert.ToInt32(a) + Convert.ToInt32(b);
                case UINT64: return Convert.ToUInt64(a) + Convert.ToUInt64(b);
                case SINT64: return Convert.ToInt64(a) + Convert.ToInt64(b);
                case FLO32: return Convert.ToSingle(a) + Convert.ToSingle(b);
                case FLO64: return Convert.ToDouble(a) + Convert.ToDouble(b);
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Dis(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8: return Convert.ToByte(a) - Convert.ToByte(b);
                case SINT_8: return Convert.ToSByte(a) - Convert.ToSByte(b);
                case UINT16: return Convert.ToUInt16(a) - Convert.ToUInt16(b);
                case SINT16: return Convert.ToInt16(a) - Convert.ToInt16(b);
                case _CHAR_: return Convert.ToChar(a) - Convert.ToChar(b);
                case UINT32: return Convert.ToUInt32(a) - Convert.ToUInt32(b);
                case SINT32: return Convert.ToInt32(a) - Convert.ToInt32(b);
                case UINT64: return Convert.ToUInt64(a) - Convert.ToUInt64(b);
                case SINT64: return Convert.ToInt64(a) - Convert.ToInt64(b);
                case FLO32: return Convert.ToSingle(a) - Convert.ToSingle(b);
                case FLO64: return Convert.ToDouble(a) - Convert.ToDouble(b);
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Mul(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8: return Convert.ToByte(a) * Convert.ToByte(b);
                case SINT_8: return Convert.ToSByte(a) * Convert.ToSByte(b);
                case UINT16: return Convert.ToUInt16(a) * Convert.ToUInt16(b);
                case SINT16: return Convert.ToInt16(a) * Convert.ToInt16(b);
                case _CHAR_: return Convert.ToChar(a) * Convert.ToChar(b);
                case UINT32: return Convert.ToUInt32(a) * Convert.ToUInt32(b);
                case SINT32: return Convert.ToInt32(a) * Convert.ToInt32(b);
                case UINT64: return Convert.ToUInt64(a) * Convert.ToUInt64(b);
                case SINT64: return Convert.ToInt64(a) * Convert.ToInt64(b);
                case FLO32: return Convert.ToSingle(a) * Convert.ToSingle(b);
                case FLO64: return Convert.ToDouble(a) * Convert.ToDouble(b);
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Div(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8: return Convert.ToByte(a) / Convert.ToByte(b);
                case SINT_8: return Convert.ToSByte(a) / Convert.ToSByte(b);
                case UINT16: return Convert.ToUInt16(a) / Convert.ToUInt16(b);
                case SINT16: return Convert.ToInt16(a) / Convert.ToInt16(b);
                case _CHAR_: return Convert.ToChar(a) / Convert.ToChar(b);
                case UINT32: return Convert.ToUInt32(a) / Convert.ToUInt32(b);
                case SINT32: return Convert.ToInt32(a) / Convert.ToInt32(b);
                case UINT64: return Convert.ToUInt64(a) / Convert.ToUInt64(b);
                case SINT64: return Convert.ToInt64(a) / Convert.ToInt64(b);
                case FLO32: return Convert.ToSingle(a) / Convert.ToSingle(b);
                case FLO64: return Convert.ToDouble(a) / Convert.ToDouble(b);
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Mod(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8: return Convert.ToByte(a) % Convert.ToByte(b);
                case SINT_8: return Convert.ToSByte(a) % Convert.ToSByte(b);
                case UINT16: return Convert.ToUInt16(a) % Convert.ToUInt16(b);
                case SINT16: return Convert.ToInt16(a) % Convert.ToInt16(b);
                case _CHAR_: return Convert.ToChar(a) % Convert.ToChar(b);
                case UINT32: return Convert.ToUInt32(a) % Convert.ToUInt32(b);
                case SINT32: return Convert.ToInt32(a) % Convert.ToInt32(b);
                case UINT64: return Convert.ToUInt64(a) % Convert.ToUInt64(b);
                case SINT64: return Convert.ToInt64(a) % Convert.ToInt64(b);
                case FLO32: return Convert.ToSingle(a) % Convert.ToSingle(b);
                case FLO64: return Convert.ToDouble(a) % Convert.ToDouble(b);
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        #endregion Math operators

        #region Math functions

        public static object Min(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8: return Math.Min(Convert.ToByte(a), Convert.ToByte(b));
                case SINT_8: return Math.Min(Convert.ToSByte(a), Convert.ToSByte(b));
                case UINT16: return Math.Min(Convert.ToUInt16(a), Convert.ToUInt16(b));
                case SINT16: return Math.Min(Convert.ToInt16(a), Convert.ToInt16(b));
                case _CHAR_: return Math.Min(Convert.ToChar(a), Convert.ToChar(b));
                case UINT32: return Math.Min(Convert.ToUInt32(a), Convert.ToUInt32(b));
                case SINT32: return Math.Min(Convert.ToInt32(a), Convert.ToInt32(b));
                case UINT64: return Math.Min(Convert.ToUInt64(a), Convert.ToUInt64(b));
                case SINT64: return Math.Min(Convert.ToInt64(a), Convert.ToInt64(b));
                case FLO32: return Math.Min(Convert.ToSingle(a), Convert.ToSingle(b));
                case FLO64: return Math.Min(Convert.ToDouble(a), Convert.ToDouble(b));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Max(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8: return Math.Max(Convert.ToByte(a), Convert.ToByte(b));
                case SINT_8: return Math.Max(Convert.ToSByte(a), Convert.ToSByte(b));
                case UINT16: return Math.Max(Convert.ToUInt16(a), Convert.ToUInt16(b));
                case SINT16: return Math.Max(Convert.ToInt16(a), Convert.ToInt16(b));
                case _CHAR_: return Math.Max(Convert.ToChar(a), Convert.ToChar(b));
                case UINT32: return Math.Max(Convert.ToUInt32(a), Convert.ToUInt32(b));
                case SINT32: return Math.Max(Convert.ToInt32(a), Convert.ToInt32(b));
                case UINT64: return Math.Max(Convert.ToUInt64(a), Convert.ToUInt64(b));
                case SINT64: return Math.Max(Convert.ToInt64(a), Convert.ToInt64(b));
                case FLO32: return Math.Max(Convert.ToSingle(a), Convert.ToSingle(b));
                case FLO64: return Math.Max(Convert.ToDouble(a), Convert.ToDouble(b));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Abs(object n)
        {
            int type = NumberType(n);
            switch (type)
            {
                case UINT_8: return Math.Abs(Convert.ToByte(n));
                case SINT_8: return Math.Abs(Convert.ToSByte(n));
                case UINT16: return Math.Abs(Convert.ToUInt16(n));
                case SINT16: return Math.Abs(Convert.ToInt16(n));
                case _CHAR_: return Math.Abs(Convert.ToChar(n));
                case UINT32: return Math.Abs(Convert.ToUInt32(n));
                case SINT32: return Math.Abs(Convert.ToInt32(n));
                case UINT64: return Math.Abs(Convert.ToDecimal(n));
                case SINT64: return Math.Abs(Convert.ToInt64(n));
                case FLO32: return Math.Abs(Convert.ToSingle(n));
                case FLO64: return Math.Abs(Convert.ToDouble(n));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Sign(object n)
        {
            int type = NumberType(n);
            switch (type)
            {
                case UINT_8: return Math.Sign(Convert.ToByte(n));
                case SINT_8: return Math.Sign(Convert.ToSByte(n));
                case UINT16: return Math.Sign(Convert.ToUInt16(n));
                case SINT16: return Math.Sign(Convert.ToInt16(n));
                case _CHAR_: return Math.Sign(Convert.ToChar(n));
                case UINT32: return Math.Sign(Convert.ToUInt32(n));
                case SINT32: return Math.Sign(Convert.ToInt32(n));
                case UINT64: return Math.Sign(Convert.ToDecimal(n));
                case SINT64: return Math.Sign(Convert.ToInt64(n));
                case FLO32: return Math.Sign(Convert.ToSingle(n));
                case FLO64: return Math.Sign(Convert.ToDouble(n));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Ceil(object n)
        {
            int type = NumberType(n);
            switch (type)
            {
                case UINT_8:
                case SINT_8:
                case UINT16:
                case SINT16:
                case _CHAR_:
                case UINT32:
                case SINT32:
                case UINT64:
                case SINT64: return Math.Ceiling(Convert.ToDecimal(n));
                case FLO32: return Math.Ceiling(Convert.ToSingle(n));
                case FLO64: return Math.Ceiling(Convert.ToDouble(n)); 
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Floor(object n)
        {
            int type = NumberType(n);
            switch (type)
            {
                case UINT_8:
                case SINT_8:
                case UINT16:
                case SINT16:
                case _CHAR_:
                case UINT32:
                case SINT32:
                case UINT64:
                case SINT64: return Math.Floor(Convert.ToDecimal(n));
                case FLO32: return Math.Floor(Convert.ToSingle(n));
                case FLO64: return Math.Floor(Convert.ToDouble(n));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }
        
        public static object Trunc(object n)
        {
            int type = NumberType(n);
            switch (type)
            {
                case UINT_8:
                case SINT_8:
                case UINT16:
                case SINT16:
                case _CHAR_:
                case UINT32:
                case SINT32:
                case UINT64:
                case SINT64: return Math.Truncate(Convert.ToDecimal(n));
                case FLO32: return Math.Truncate(Convert.ToSingle(n));
                case FLO64: return Math.Truncate(Convert.ToDouble(n));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Sqrt(object n)
        {
            int type = NumberType(n);
            switch (type)
            {
                case UINT_8: return Math.Sqrt(Convert.ToByte(n));
                case SINT_8: return Math.Sqrt(Convert.ToSByte(n));
                case UINT16: return Math.Sqrt(Convert.ToUInt16(n));
                case SINT16: return Math.Sqrt(Convert.ToInt16(n));
                case _CHAR_: return Math.Sqrt(Convert.ToChar(n));
                case UINT32: return Math.Sqrt(Convert.ToUInt32(n));
                case SINT32: return Math.Sqrt(Convert.ToInt32(n));
                case UINT64: return Math.Sqrt(Convert.ToUInt64(n));
                case SINT64: return Math.Sqrt(Convert.ToInt64(n));
                case FLO32: return Math.Sqrt(Convert.ToSingle(n));
                case FLO64: return Math.Sqrt(Convert.ToDouble(n));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Pow(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8:
                case SINT_8:
                case UINT16:
                case SINT16:
                case _CHAR_:
                case UINT32:
                case SINT32:
                case UINT64:
                case SINT64:
                case FLO32:
                case FLO64: return Math.Pow(Convert.ToDouble(a), Convert.ToDouble(b));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Exp(object n)
        {
            int type = NumberType(n);
            switch (type)
            {
                case UINT_8:
                case SINT_8:
                case UINT16:
                case SINT16:
                case _CHAR_:
                case UINT32:
                case SINT32:
                case UINT64:
                case SINT64:
                case FLO32:
                case FLO64: return Math.Exp(Convert.ToDouble(n));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Log(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8:
                case SINT_8:
                case UINT16:
                case SINT16:
                case _CHAR_:
                case UINT32:
                case SINT32:
                case UINT64:
                case SINT64:
                case FLO32:
                case FLO64: return Math.Log(Convert.ToDouble(a), Convert.ToDouble(b));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Logn(object n)
        {
            int type = NumberType(n);
            switch (type)
            {
                case UINT_8:
                case SINT_8:
                case UINT16:
                case SINT16:
                case _CHAR_:
                case UINT32:
                case SINT32:
                case UINT64:
                case SINT64:
                case FLO32:
                case FLO64: return Math.Log(Convert.ToDouble(n));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Log10(object n)
        {
            int type = NumberType(n);
            switch (type)
            {
                case UINT_8:
                case SINT_8:
                case UINT16:
                case SINT16:
                case _CHAR_:
                case UINT32:
                case SINT32:
                case UINT64:
                case SINT64:
                case FLO32:
                case FLO64: return Math.Log10(Convert.ToDouble(n));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }
        
        public static object Sin(object n)
        {
            int type = NumberType(n);
            switch (type)
            {
                case UINT_8:
                case SINT_8:
                case UINT16:
                case SINT16:
                case _CHAR_:
                case UINT32:
                case SINT32:
                case UINT64:
                case SINT64:
                case FLO32:
                case FLO64: return Math.Sin(Convert.ToDouble(n));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Cos(object n)
        {
            int type = NumberType(n);
            switch (type)
            {
                case UINT_8:
                case SINT_8:
                case UINT16:
                case SINT16:
                case _CHAR_:
                case UINT32:
                case SINT32:
                case UINT64:
                case SINT64:
                case FLO32:
                case FLO64: return Math.Cos(Convert.ToDouble(n));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Tan(object n)
        {
            int type = NumberType(n);
            switch (type)
            {
                case UINT_8:
                case SINT_8:
                case UINT16:
                case SINT16:
                case _CHAR_:
                case UINT32:
                case SINT32:
                case UINT64:
                case SINT64:
                case FLO32:
                case FLO64: return Math.Tan(Convert.ToDouble(n));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }
        
        public static object Asin(object n)
        {
            int type = NumberType(n);
            switch (type)
            {
                case UINT_8:
                case SINT_8:
                case UINT16:
                case SINT16:
                case _CHAR_:
                case UINT32:
                case SINT32:
                case UINT64:
                case SINT64:
                case FLO32:
                case FLO64: return Math.Asin(Convert.ToDouble(n));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Acos(object n)
        {
            int type = NumberType(n);
            switch (type)
            {
                case UINT_8:
                case SINT_8:
                case UINT16:
                case SINT16:
                case _CHAR_:
                case UINT32:
                case SINT32:
                case UINT64:
                case SINT64:
                case FLO32:
                case FLO64: return Math.Acos(Convert.ToDouble(n));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Atan(object n)
        {
            int type = NumberType(n);
            switch (type)
            {
                case UINT_8:
                case SINT_8:
                case UINT16:
                case SINT16:
                case _CHAR_:
                case UINT32:
                case SINT32:
                case UINT64:
                case SINT64:
                case FLO32:
                case FLO64: return Math.Atan(Convert.ToDouble(n));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Atan2(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8:
                case SINT_8:
                case UINT16:
                case SINT16:
                case _CHAR_:
                case UINT32:
                case SINT32:
                case UINT64:
                case SINT64:
                case FLO32:
                case FLO64: return Math.Atan2(Convert.ToDouble(a), Convert.ToDouble(b));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }
        
        public static object Sinh(object n)
        {
            int type = NumberType(n);
            switch (type)
            {
                case UINT_8:
                case SINT_8:
                case UINT16:
                case SINT16:
                case _CHAR_:
                case UINT32:
                case SINT32:
                case UINT64:
                case SINT64:
                case FLO32:
                case FLO64: return Math.Sinh(Convert.ToDouble(n));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Cosh(object n)
        {
            int type = NumberType(n);
            switch (type)
            {
                case UINT_8:
                case SINT_8:
                case UINT16:
                case SINT16:
                case _CHAR_:
                case UINT32:
                case SINT32:
                case UINT64:
                case SINT64:
                case FLO32:
                case FLO64: return Math.Cosh(Convert.ToDouble(n));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Tanh(object n)
        {
            int type = NumberType(n);
            switch (type)
            {
                case UINT_8:
                case SINT_8:
                case UINT16:
                case SINT16:
                case _CHAR_:
                case UINT32:
                case SINT32:
                case UINT64:
                case SINT64:
                case FLO32:
                case FLO64: return Math.Tanh(Convert.ToDouble(n));
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }
        
        #endregion

        #region Binary operators

        public static object And(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8: return Convert.ToByte(a) & Convert.ToByte(b);
                case SINT_8: return Convert.ToSByte(a) & Convert.ToSByte(b);
                case UINT16: return Convert.ToUInt16(a) & Convert.ToUInt16(b);
                case SINT16: return Convert.ToInt16(a) & Convert.ToInt16(b);
                case _CHAR_: return Convert.ToChar(a) & Convert.ToChar(b);
                case UINT32: return Convert.ToUInt32(a) & Convert.ToUInt32(b);
                case SINT32: return Convert.ToInt32(a) & Convert.ToInt32(b);
                case UINT64: return Convert.ToUInt64(a) & Convert.ToUInt64(b);
                case SINT64: return Convert.ToInt64(a) & Convert.ToInt64(b);
                case FLO32:
                case FLO64:
                    throw new InvalidOperationException("Operator & not applicable to floating-point numbers");
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Or(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8: return Convert.ToByte(a) | Convert.ToByte(b);
                case SINT_8: return Convert.ToSByte(a) | Convert.ToSByte(b);
                case UINT16: return Convert.ToUInt16(a) | Convert.ToUInt16(b);
                case SINT16: return Convert.ToInt16(a) | Convert.ToInt16(b);
                case _CHAR_: return Convert.ToChar(a) | Convert.ToChar(b);
                case UINT32: return Convert.ToUInt32(a) | Convert.ToUInt32(b);
                case SINT32: return Convert.ToInt32(a) | Convert.ToInt32(b);
                case UINT64: return Convert.ToUInt64(a) | Convert.ToUInt64(b);
                case SINT64: return Convert.ToInt64(a) | Convert.ToInt64(b);
                case FLO32:
                case FLO64:
                    throw new InvalidOperationException("Operator | not applicable to floating-point numbers");
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Xor(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8: return Convert.ToByte(a) ^ Convert.ToByte(b);
                case SINT_8: return Convert.ToSByte(a) ^ Convert.ToSByte(b);
                case UINT16: return Convert.ToUInt16(a) ^ Convert.ToUInt16(b);
                case SINT16: return Convert.ToInt16(a) ^ Convert.ToInt16(b);
                case _CHAR_: return Convert.ToChar(a) ^ Convert.ToChar(b);
                case UINT32: return Convert.ToUInt32(a) ^ Convert.ToUInt32(b);
                case SINT32: return Convert.ToInt32(a) ^ Convert.ToInt32(b);
                case UINT64: return Convert.ToUInt64(a) ^ Convert.ToUInt64(b);
                case SINT64: return Convert.ToInt64(a) ^ Convert.ToInt64(b);
                case FLO32:
                case FLO64:
                    throw new InvalidOperationException("Operator ^ not applicable to floating-point numbers");
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Lsh(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8: return Convert.ToByte(a) << Convert.ToByte(b);
                case SINT_8: return Convert.ToSByte(a) << Convert.ToSByte(b);
                case UINT16: return Convert.ToUInt16(a) << Convert.ToUInt16(b);
                case SINT16: return Convert.ToInt16(a) << Convert.ToInt16(b);
                case _CHAR_: return Convert.ToChar(a) << Convert.ToChar(b);
                case UINT32: return Convert.ToUInt32(a) << Convert.ToInt32(b);
                case SINT32: return Convert.ToInt32(a) << Convert.ToInt32(b);
                case UINT64: return Convert.ToUInt64(a) << Convert.ToInt32(b);
                case SINT64: return Convert.ToInt64(a) << Convert.ToInt32(b);
                case FLO32:
                case FLO64:
                    throw new InvalidOperationException("Operator << not applicable to floating-point numbers");
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static object Rsh(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8: return Convert.ToByte(a) >> Convert.ToByte(b);
                case SINT_8: return Convert.ToSByte(a) >> Convert.ToSByte(b);
                case UINT16: return Convert.ToUInt16(a) >> Convert.ToUInt16(b);
                case SINT16: return Convert.ToInt16(a) >> Convert.ToInt16(b);
                case _CHAR_: return Convert.ToChar(a) >> Convert.ToChar(b);
                case UINT32: return Convert.ToUInt32(a) >> Convert.ToInt32(b);
                case SINT32: return Convert.ToInt32(a) >> Convert.ToInt32(b);
                case UINT64: return Convert.ToUInt64(a) >> Convert.ToInt32(b);
                case SINT64: return Convert.ToInt64(a) >> Convert.ToInt32(b);
                case FLO32:
                case FLO64:
                    throw new InvalidOperationException("Operator >> not applicable to floating-point numbers");
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        #endregion Binary operators

        #region Logic operators

        public static bool Eq(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8: return Convert.ToByte(a) == Convert.ToByte(b);
                case SINT_8: return Convert.ToSByte(a) == Convert.ToSByte(b);
                case UINT16: return Convert.ToUInt16(a) == Convert.ToUInt16(b);
                case SINT16: return Convert.ToInt16(a) == Convert.ToInt16(b);
                case _CHAR_: return Convert.ToChar(a) == Convert.ToChar(b);
                case UINT32: return Convert.ToUInt32(a) == Convert.ToUInt32(b);
                case SINT32: return Convert.ToInt32(a) == Convert.ToInt32(b);
                case UINT64: return Convert.ToUInt64(a) == Convert.ToUInt64(b);
                case SINT64: return Convert.ToInt64(a) == Convert.ToInt64(b);
                case FLO32: return Convert.ToSingle(a) == Convert.ToSingle(b);
                case FLO64: return Convert.ToDouble(a) == Convert.ToDouble(b);
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static bool Lt(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8: return Convert.ToByte(a) < Convert.ToByte(b);
                case SINT_8: return Convert.ToSByte(a) < Convert.ToSByte(b);
                case UINT16: return Convert.ToUInt16(a) < Convert.ToUInt16(b);
                case SINT16: return Convert.ToInt16(a) < Convert.ToInt16(b);
                case _CHAR_: return Convert.ToChar(a) < Convert.ToChar(b);
                case UINT32: return Convert.ToUInt32(a) < Convert.ToUInt32(b);
                case SINT32: return Convert.ToInt32(a) < Convert.ToInt32(b);
                case UINT64: return Convert.ToUInt64(a) < Convert.ToUInt64(b);
                case SINT64: return Convert.ToInt64(a) < Convert.ToInt64(b);
                case FLO32: return Convert.ToSingle(a) < Convert.ToSingle(b);
                case FLO64: return Convert.ToDouble(a) < Convert.ToDouble(b);
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static bool Gt(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8: return Convert.ToByte(a) > Convert.ToByte(b);
                case SINT_8: return Convert.ToSByte(a) > Convert.ToSByte(b);
                case UINT16: return Convert.ToUInt16(a) > Convert.ToUInt16(b);
                case SINT16: return Convert.ToInt16(a) > Convert.ToInt16(b);
                case _CHAR_: return Convert.ToChar(a) > Convert.ToChar(b);
                case UINT32: return Convert.ToUInt32(a) > Convert.ToUInt32(b);
                case SINT32: return Convert.ToInt32(a) > Convert.ToInt32(b);
                case UINT64: return Convert.ToUInt64(a) > Convert.ToUInt64(b);
                case SINT64: return Convert.ToInt64(a) > Convert.ToInt64(b);
                case FLO32: return Convert.ToSingle(a) > Convert.ToSingle(b);
                case FLO64: return Convert.ToDouble(a) > Convert.ToDouble(b);
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static bool Le(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8: return Convert.ToByte(a) <= Convert.ToByte(b);
                case SINT_8: return Convert.ToSByte(a) <= Convert.ToSByte(b);
                case UINT16: return Convert.ToUInt16(a) <= Convert.ToUInt16(b);
                case SINT16: return Convert.ToInt16(a) <= Convert.ToInt16(b);
                case _CHAR_: return Convert.ToChar(a) <= Convert.ToChar(b);
                case UINT32: return Convert.ToUInt32(a) <= Convert.ToUInt32(b);
                case SINT32: return Convert.ToInt32(a) <= Convert.ToInt32(b);
                case UINT64: return Convert.ToUInt64(a) <= Convert.ToUInt64(b);
                case SINT64: return Convert.ToInt64(a) <= Convert.ToInt64(b);
                case FLO32: return Convert.ToSingle(a) <= Convert.ToSingle(b);
                case FLO64: return Convert.ToDouble(a) <= Convert.ToDouble(b);
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        public static bool Ge(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8: return Convert.ToByte(a) >= Convert.ToByte(b);
                case SINT_8: return Convert.ToSByte(a) >= Convert.ToSByte(b);
                case UINT16: return Convert.ToUInt16(a) >= Convert.ToUInt16(b);
                case SINT16: return Convert.ToInt16(a) >= Convert.ToInt16(b);
                case _CHAR_: return Convert.ToChar(a) >= Convert.ToChar(b);
                case UINT32: return Convert.ToUInt32(a) >= Convert.ToUInt32(b);
                case SINT32: return Convert.ToInt32(a) >= Convert.ToInt32(b);
                case UINT64: return Convert.ToUInt64(a) >= Convert.ToUInt64(b);
                case SINT64: return Convert.ToInt64(a) >= Convert.ToInt64(b);
                case FLO32: return Convert.ToSingle(a) >= Convert.ToSingle(b);
                case FLO64: return Convert.ToDouble(a) >= Convert.ToDouble(b);
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }
        
        public static bool Ne(object a, object b)
        {
            int type = MaxNumberType(a, b);
            switch (type)
            {
                case UINT_8: return Convert.ToByte(a) != Convert.ToByte(b);
                case SINT_8: return Convert.ToSByte(a) != Convert.ToSByte(b);
                case UINT16: return Convert.ToUInt16(a) != Convert.ToUInt16(b);
                case SINT16: return Convert.ToInt16(a) != Convert.ToInt16(b);
                case _CHAR_: return Convert.ToChar(a) != Convert.ToChar(b);
                case UINT32: return Convert.ToUInt32(a) != Convert.ToUInt32(b);
                case SINT32: return Convert.ToInt32(a) != Convert.ToInt32(b);
                case UINT64: return Convert.ToUInt64(a) != Convert.ToUInt64(b);
                case SINT64: return Convert.ToInt64(a) != Convert.ToInt64(b);
                case FLO32: return Convert.ToSingle(a) != Convert.ToSingle(b);
                case FLO64: return Convert.ToDouble(a) != Convert.ToDouble(b);
                default:
                    throw new InvalidOperationException("Can't apply operator to a not-number!");
            }
        }

        #endregion Logic operators
    }
}