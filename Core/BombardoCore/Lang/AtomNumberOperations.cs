using System;

// ReSharper disable RedundantCast

namespace Bombardo.Core
{
    internal class AtomNumberOperations
    {
        public static bool    useCSUpTypeOperations = false;
        public static bool    raiseNotANumber       = true;
        public static float   floatTolerance        = 0.000001f;
        public static double  doubleTolerance       = 0.000000000001;
        public static decimal decimalTolerance      = 0.000000000000000000000001m;

        public static bool Compare(float a, float b)
        {
            return Math.Abs(a - b) < floatTolerance;
        }

        public static bool Compare(double a, double b)
        {
            return Math.Abs(a - b) < doubleTolerance;
        }

        public static bool Compare(decimal a, decimal b)
        {
            return Math.Abs(a - b) < decimalTolerance;
        }

        public static int MaxNumberType(AtomNumber a, AtomNumber b)
        {
            return Math.Max(a.type, b.type);
        }

#region Math operators

        public static AtomNumber Sum(AtomNumber a, AtomNumber b)
        {
            AtomNumber result = new AtomNumber { type = MaxNumberType(a, b) };
            // CS type operations upcast
            if (useCSUpTypeOperations &&
                result.type < AtomNumberType.SINT32)
                result.type = AtomNumberType.SINT32;
            // @formatter:off
            switch ( result.type )
            {
                case AtomNumberType.UINT_8: result.val_uint8    = (byte)    (a.ToUByte()   + b.ToUByte());   break;
                case AtomNumberType.SINT_8: result.val_sint8    = (sbyte)   (a.ToSByte()   + b.ToSByte());   break;
                case AtomNumberType.UINT16: result.val_uint16   = (ushort)  (a.ToUShort()  + b.ToUShort());  break;
                case AtomNumberType.SINT16: result.val_sint16   = (short)   (a.ToSShort()  + b.ToSShort());  break;
                case AtomNumberType._CHAR_: result.val_char     = (char)    (a.ToChar()    + b.ToChar());    break;
                case AtomNumberType.UINT32: result.val_uint32   = (uint)    (a.ToUInt()    + b.ToUInt());    break;
                case AtomNumberType.SINT32: result.val_sint32   = (int)     (a.ToSInt()    + b.ToSInt());    break;
                case AtomNumberType.UINT64: result.val_uint64   = (ulong)   (a.ToULong()   + b.ToULong());   break;
                case AtomNumberType.SINT64: result.val_sint64   = (long)    (a.ToSLong()   + b.ToSLong());   break;
                case AtomNumberType.SINGLE: result.val_single   = (float)   (a.ToSingle()  + b.ToSingle());  break;
                case AtomNumberType.DOUBLE: result.val_double   = (double)  (a.ToDouble()  + b.ToDouble());  break;
                case AtomNumberType.DECIMAL: result.val_decimal = (decimal) (a.ToDecimal() + b.ToDecimal()); break;
                case AtomNumberType.NaN:
                    if (!raiseNotANumber)
                        throw new InvalidOperationException("Can't apply operator to a not-number!");
                    break;
                
            }
            // @formatter:on
            return result;
        }

        public static AtomNumber Dis(AtomNumber a, AtomNumber b)
        {
            AtomNumber result = new AtomNumber { type = MaxNumberType(a, b) };
            // CS type operations upcast
            if (useCSUpTypeOperations &&
                result.type < AtomNumberType.SINT32)
                result.type = AtomNumberType.SINT32;
            // @formatter:off
            switch ( result.type )
            {
                case AtomNumberType.UINT_8: result.val_uint8    = (byte)    (a.ToUByte()   - b.ToUByte());   break;
                case AtomNumberType.SINT_8: result.val_sint8    = (sbyte)   (a.ToSByte()   - b.ToSByte());   break;
                case AtomNumberType.UINT16: result.val_uint16   = (ushort)  (a.ToUShort()  - b.ToUShort());  break;
                case AtomNumberType.SINT16: result.val_sint16   = (short)   (a.ToSShort()  - b.ToSShort());  break;
                case AtomNumberType._CHAR_: result.val_char     = (char)    (a.ToChar()    - b.ToChar());    break;
                case AtomNumberType.UINT32: result.val_uint32   = (uint)    (a.ToUInt()    - b.ToUInt());    break;
                case AtomNumberType.SINT32: result.val_sint32   = (int)     (a.ToSInt()    - b.ToSInt());    break;
                case AtomNumberType.UINT64: result.val_uint64   = (ulong)   (a.ToULong()   - b.ToULong());   break;
                case AtomNumberType.SINT64: result.val_sint64   = (long)    (a.ToSLong()   - b.ToSLong());   break;
                case AtomNumberType.SINGLE: result.val_single   = (float)   (a.ToSingle()  - b.ToSingle());  break;
                case AtomNumberType.DOUBLE: result.val_double   = (double)  (a.ToDouble()  - b.ToDouble());  break;
                case AtomNumberType.DECIMAL: result.val_decimal = (decimal) (a.ToDecimal() - b.ToDecimal()); break;
                case AtomNumberType.NaN:
                    if (!raiseNotANumber)
                        throw new InvalidOperationException("Can't apply operator to a not-number!");
                    break;
            }
            // @formatter:on
            return result;
        }

        public static AtomNumber Mul(AtomNumber a, AtomNumber b)
        {
            AtomNumber result = new AtomNumber { type = MaxNumberType(a, b) };
            // CS type operations upcast
            if (useCSUpTypeOperations &&
                result.type < AtomNumberType.SINT32)
                result.type = AtomNumberType.SINT32;
            // @formatter:off
            switch ( result.type )
            {
                case AtomNumberType.UINT_8: result.val_uint8    = (byte)    (a.ToUByte()   * b.ToUByte());   break;
                case AtomNumberType.SINT_8: result.val_sint8    = (sbyte)   (a.ToSByte()   * b.ToSByte());   break;
                case AtomNumberType.UINT16: result.val_uint16   = (ushort)  (a.ToUShort()  * b.ToUShort());  break;
                case AtomNumberType.SINT16: result.val_sint16   = (short)   (a.ToSShort()  * b.ToSShort());  break;
                case AtomNumberType._CHAR_: result.val_char     = (char)    (a.ToChar()    * b.ToChar());    break;
                case AtomNumberType.UINT32: result.val_uint32   = (uint)    (a.ToUInt()    * b.ToUInt());    break;
                case AtomNumberType.SINT32: result.val_sint32   = (int)     (a.ToSInt()    * b.ToSInt());    break;
                case AtomNumberType.UINT64: result.val_uint64   = (ulong)   (a.ToULong()   * b.ToULong());   break;
                case AtomNumberType.SINT64: result.val_sint64   = (long)    (a.ToSLong()   * b.ToSLong());   break;
                case AtomNumberType.SINGLE: result.val_single   = (float)   (a.ToSingle()  * b.ToSingle());  break;
                case AtomNumberType.DOUBLE: result.val_double   = (double)  (a.ToDouble()  * b.ToDouble());  break;
                case AtomNumberType.DECIMAL: result.val_decimal = (decimal) (a.ToDecimal() * b.ToDecimal()); break;
                case AtomNumberType.NaN:
                    if (!raiseNotANumber)
                        throw new InvalidOperationException("Can't apply operator to a not-number!");
                    break;
            }
            // @formatter:on
            return result;
        }

        public static AtomNumber Div(AtomNumber a, AtomNumber b)
        {
            AtomNumber result = new AtomNumber { type = MaxNumberType(a, b) };
            // CS type operations upcast
            if (useCSUpTypeOperations &&
                result.type < AtomNumberType.SINT32)
                result.type = AtomNumberType.SINT32;
            // @formatter:off
            switch ( result.type )
            {
                case AtomNumberType.UINT_8: result.val_uint8    = (byte)    (a.ToUByte()   / b.ToUByte());   break;
                case AtomNumberType.SINT_8: result.val_sint8    = (sbyte)   (a.ToSByte()   / b.ToSByte());   break;
                case AtomNumberType.UINT16: result.val_uint16   = (ushort)  (a.ToUShort()  / b.ToUShort());  break;
                case AtomNumberType.SINT16: result.val_sint16   = (short)   (a.ToSShort()  / b.ToSShort());  break;
                case AtomNumberType._CHAR_: result.val_char     = (char)    (a.ToChar()    / b.ToChar());    break;
                case AtomNumberType.UINT32: result.val_uint32   = (uint)    (a.ToUInt()    / b.ToUInt());    break;
                case AtomNumberType.SINT32: result.val_sint32   = (int)     (a.ToSInt()    / b.ToSInt());    break;
                case AtomNumberType.UINT64: result.val_uint64   = (ulong)   (a.ToULong()   / b.ToULong());   break;
                case AtomNumberType.SINT64: result.val_sint64   = (long)    (a.ToSLong()   / b.ToSLong());   break;
                case AtomNumberType.SINGLE: result.val_single   = (float)   (a.ToSingle()  / b.ToSingle());  break;
                case AtomNumberType.DOUBLE: result.val_double   = (double)  (a.ToDouble()  / b.ToDouble());  break;
                case AtomNumberType.DECIMAL: result.val_decimal = (decimal) (a.ToDecimal() / b.ToDecimal()); break;
                case AtomNumberType.NaN:
                    if (!raiseNotANumber)
                        throw new InvalidOperationException("Can't apply operator to a not-number!");
                    break;
            }
            // @formatter:on
            return result;
        }

        public static AtomNumber Mod(AtomNumber a, AtomNumber b)
        {
            AtomNumber result = new AtomNumber { type = MaxNumberType(a, b) };
            // CS type operations upcast
            if (useCSUpTypeOperations &&
                result.type < AtomNumberType.SINT32)
                result.type = AtomNumberType.SINT32;
            // @formatter:off
            switch ( result.type )
            {
                case AtomNumberType.UINT_8: result.val_uint8    = (byte)    (a.ToUByte()   % b.ToUByte());   break;
                case AtomNumberType.SINT_8: result.val_sint8    = (sbyte)   (a.ToSByte()   % b.ToSByte());   break;
                case AtomNumberType.UINT16: result.val_uint16   = (ushort)  (a.ToUShort()  % b.ToUShort());  break;
                case AtomNumberType.SINT16: result.val_sint16   = (short)   (a.ToSShort()  % b.ToSShort());  break;
                case AtomNumberType._CHAR_: result.val_char     = (char)    (a.ToChar()    % b.ToChar());    break;
                case AtomNumberType.UINT32: result.val_uint32   = (uint)    (a.ToUInt()    % b.ToUInt());    break;
                case AtomNumberType.SINT32: result.val_sint32   = (int)     (a.ToSInt()    % b.ToSInt());    break;
                case AtomNumberType.UINT64: result.val_uint64   = (ulong)   (a.ToULong()   % b.ToULong());   break;
                case AtomNumberType.SINT64: result.val_sint64   = (long)    (a.ToSLong()   % b.ToSLong());   break;
                case AtomNumberType.SINGLE: result.val_single   = (float)   (a.ToSingle()  % b.ToSingle());  break;
                case AtomNumberType.DOUBLE: result.val_double   = (double)  (a.ToDouble()  % b.ToDouble());  break;
                case AtomNumberType.DECIMAL: result.val_decimal = (decimal) (a.ToDecimal() % b.ToDecimal()); break;
                case AtomNumberType.NaN:
                    if (!raiseNotANumber)
                        throw new InvalidOperationException("Can't apply operator to a not-number!");
                    break;
            }
            // @formatter:on
            return result;
        }

#endregion Math operators


#region Math functions

        public static AtomNumber Min(AtomNumber a, AtomNumber b)
        {
            AtomNumber result = new AtomNumber { type = MaxNumberType(a, b) };
            // CS type operations upcast
            if (useCSUpTypeOperations &&
                result.type < AtomNumberType.SINT32)
                result.type = AtomNumberType.SINT32;
            // @formatter:off
            switch ( result.type )
            {
                case AtomNumberType.UINT_8:  result.val_uint8   =        Math.Min (a.ToUByte(),   b.ToUByte());   break;
                case AtomNumberType.SINT_8:  result.val_sint8   =        Math.Min (a.ToSByte(),   b.ToSByte());   break;
                case AtomNumberType.UINT16:  result.val_uint16  =        Math.Min (a.ToUShort(),  b.ToUShort());  break;
                case AtomNumberType.SINT16:  result.val_sint16  =        Math.Min (a.ToSShort(),  b.ToSShort());  break;
                case AtomNumberType._CHAR_:  result.val_char    = (char) Math.Min (a.ToChar(),    b.ToChar());    break;
                case AtomNumberType.UINT32:  result.val_uint32  =        Math.Min (a.ToUInt(),    b.ToUInt());    break;
                case AtomNumberType.SINT32:  result.val_sint32  =        Math.Min (a.ToSInt(),    b.ToSInt());    break;
                case AtomNumberType.UINT64:  result.val_uint64  =        Math.Min (a.ToULong(),   b.ToULong());   break;
                case AtomNumberType.SINT64:  result.val_sint64  =        Math.Min (a.ToSLong(),   b.ToSLong());   break;
                case AtomNumberType.SINGLE:  result.val_single  =        Math.Min (a.ToSingle(),  b.ToSingle());  break;
                case AtomNumberType.DOUBLE:  result.val_double  =        Math.Min (a.ToDouble(),  b.ToDouble());  break;
                case AtomNumberType.DECIMAL: result.val_decimal =        Math.Min (a.ToDecimal(), b.ToDecimal()); break;
                case AtomNumberType.NaN:
                    if (!raiseNotANumber)
                        throw new InvalidOperationException("Can't apply operator to a not-number!");
                    break;
            }
            // @formatter:on
            return result;
        }

        public static AtomNumber Max(AtomNumber a, AtomNumber b)
        {
            AtomNumber result = new AtomNumber { type = MaxNumberType(a, b) };
            // CS type operations upcast
            if (useCSUpTypeOperations &&
                result.type < AtomNumberType.SINT32)
                result.type = AtomNumberType.SINT32;
            // @formatter:off
            switch ( result.type )
            {
                case AtomNumberType.UINT_8:  result.val_uint8   =        Math.Max (a.ToUByte(),   b.ToUByte());   break;
                case AtomNumberType.SINT_8:  result.val_sint8   =        Math.Max (a.ToSByte(),   b.ToSByte());   break;
                case AtomNumberType.UINT16:  result.val_uint16  =        Math.Max (a.ToUShort(),  b.ToUShort());  break;
                case AtomNumberType.SINT16:  result.val_sint16  =        Math.Max (a.ToSShort(),  b.ToSShort());  break;
                case AtomNumberType._CHAR_:  result.val_char    = (char) Math.Max (a.ToChar(),    b.ToChar());    break;
                case AtomNumberType.UINT32:  result.val_uint32  =        Math.Max (a.ToUInt(),    b.ToUInt());    break;
                case AtomNumberType.SINT32:  result.val_sint32  =        Math.Max (a.ToSInt(),    b.ToSInt());    break;
                case AtomNumberType.UINT64:  result.val_uint64  =        Math.Max (a.ToULong(),   b.ToULong());   break;
                case AtomNumberType.SINT64:  result.val_sint64  =        Math.Max (a.ToSLong(),   b.ToSLong());   break;
                case AtomNumberType.SINGLE:  result.val_single  =        Math.Max (a.ToSingle(),  b.ToSingle());  break;
                case AtomNumberType.DOUBLE:  result.val_double  =        Math.Max (a.ToDouble(),  b.ToDouble());  break;
                case AtomNumberType.DECIMAL: result.val_decimal =        Math.Max (a.ToDecimal(), b.ToDecimal()); break;
                case AtomNumberType.NaN:
                    if (!raiseNotANumber)
                        throw new InvalidOperationException("Can't apply operator to a not-number!");
                    break;
            }
            // @formatter:on
            return result;
        }

        public static AtomNumber Abs(AtomNumber n)
        {
            AtomNumber result = new AtomNumber { type = n.type };
            // @formatter:off
            switch ( result.type )
            {
                case AtomNumberType.UINT_8:  result.val_uint8   =           n.ToUByte();    break;
                case AtomNumberType.SINT_8:  result.val_sint8   = Math.Abs (n.ToSByte());   break;
                case AtomNumberType.UINT16:  result.val_uint16  =           n.ToUShort();   break;
                case AtomNumberType.SINT16:  result.val_sint16  = Math.Abs (n.ToSShort());  break;
                case AtomNumberType._CHAR_:  result.val_char    =           n.ToChar();     break;
                case AtomNumberType.UINT32:  result.val_uint32  =           n.ToUInt();     break;
                case AtomNumberType.SINT32:  result.val_sint32  = Math.Abs (n.ToSInt());    break;
                case AtomNumberType.UINT64:  result.val_uint64  =           n.ToULong();    break;
                case AtomNumberType.SINT64:  result.val_sint64  = Math.Abs (n.ToSLong());   break;
                case AtomNumberType.SINGLE:  result.val_single  = Math.Abs (n.ToSingle());  break;
                case AtomNumberType.DOUBLE:  result.val_double  = Math.Abs (n.ToDouble());  break;
                case AtomNumberType.DECIMAL: result.val_decimal = Math.Abs (n.ToDecimal()); break;
                case AtomNumberType.NaN:
                    if (!raiseNotANumber)
                        throw new InvalidOperationException("Can't apply operator to a not-number!");
                    break;
            }
            // @formatter:on
            return result;
        }

        public static AtomNumber Sign(AtomNumber n)
        {
            AtomNumber result = new AtomNumber { type = AtomNumberType.SINT32 };
            // @formatter:off
            switch ( n.type )
            {
                case AtomNumberType.UINT_8:  result.val_sint32 =            1;              break;
                case AtomNumberType.SINT_8:  result.val_sint32 = Math.Sign (n.ToSByte());   break;
                case AtomNumberType.UINT16:  result.val_sint32 =            1;              break;
                case AtomNumberType.SINT16:  result.val_sint32 = Math.Sign (n.ToSShort());  break;
                case AtomNumberType._CHAR_:  result.val_sint32 =            1;              break;
                case AtomNumberType.UINT32:  result.val_sint32 =            1;              break;
                case AtomNumberType.SINT32:  result.val_sint32 = Math.Sign (n.ToSInt());    break;
                case AtomNumberType.UINT64:  result.val_sint32 =            1;              break;
                case AtomNumberType.SINT64:  result.val_sint32 = Math.Sign (n.ToSLong());   break;
                case AtomNumberType.SINGLE:  result.val_sint32 = Math.Sign (n.ToSingle());  break;
                case AtomNumberType.DOUBLE:  result.val_sint32 = Math.Sign (n.ToDouble());  break;
                case AtomNumberType.DECIMAL: result.val_sint32 = Math.Sign (n.ToDecimal()); break;
                case AtomNumberType.NaN:
                    if (!raiseNotANumber)
                        throw new InvalidOperationException("Can't apply operator to a not-number!");
                    break;
            }
            // @formatter:on
            return result;
        }

        public static AtomNumber Ceil(AtomNumber n)
        {
            AtomNumber result = new AtomNumber { type = n.type };
            // @formatter:off
            switch ( n.type )
            {
                case AtomNumberType.UINT_8:  result.val_uint8   =                      n.val_uint8;    break;
                case AtomNumberType.SINT_8:  result.val_sint8   =                      n.val_sint8;    break;
                case AtomNumberType.UINT16:  result.val_uint16  =                      n.val_uint16;   break;
                case AtomNumberType.SINT16:  result.val_sint16  =                      n.val_sint16;   break;
                case AtomNumberType._CHAR_:  result.val_char    =                      n.val_char;     break;
                case AtomNumberType.UINT32:  result.val_sint32  =                      n.val_sint32;   break;
                case AtomNumberType.SINT32:  result.val_uint32  =                      n.val_uint32;   break;
                case AtomNumberType.UINT64:  result.val_sint64  =                      n.val_sint64;   break;
                case AtomNumberType.SINT64:  result.val_uint64  =                      n.val_uint64;   break;
                case AtomNumberType.SINGLE:  result.val_single  = (float) Math.Ceiling(n.ToSingle());  break;
                case AtomNumberType.DOUBLE:  result.val_double  =         Math.Ceiling(n.ToDouble());  break;
                case AtomNumberType.DECIMAL: result.val_decimal =         Math.Ceiling(n.ToDecimal()); break;
                case AtomNumberType.NaN:
                    if (!raiseNotANumber)
                        throw new InvalidOperationException("Can't apply operator to a not-number!");
                    break;
            }
            // @formatter:on
            return result;
        }

        public static AtomNumber Floor(AtomNumber n)
        {
            AtomNumber result = new AtomNumber { type = n.type };
            // @formatter:off
            switch ( n.type )
            {
                case AtomNumberType.UINT_8:  result.val_uint8   =                    n.val_uint8;    break;
                case AtomNumberType.SINT_8:  result.val_sint8   =                    n.val_sint8;    break;
                case AtomNumberType.UINT16:  result.val_uint16  =                    n.val_uint16;   break;
                case AtomNumberType.SINT16:  result.val_sint16  =                    n.val_sint16;   break;
                case AtomNumberType._CHAR_:  result.val_char    =                    n.val_char;     break;
                case AtomNumberType.UINT32:  result.val_sint32  =                    n.val_sint32;   break;
                case AtomNumberType.SINT32:  result.val_uint32  =                    n.val_uint32;   break;
                case AtomNumberType.UINT64:  result.val_sint64  =                    n.val_sint64;   break;
                case AtomNumberType.SINT64:  result.val_uint64  =                    n.val_uint64;   break;
                case AtomNumberType.SINGLE:  result.val_single  = (float) Math.Floor(n.ToSingle());  break;
                case AtomNumberType.DOUBLE:  result.val_double  =         Math.Floor(n.ToDouble());  break;
                case AtomNumberType.DECIMAL: result.val_decimal =         Math.Floor(n.ToDecimal()); break;
                case AtomNumberType.NaN:
                    if (!raiseNotANumber)
                        throw new InvalidOperationException("Can't apply operator to a not-number!");
                    break;
            }
            // @formatter:on
            return result;
        }

        public static AtomNumber Trunc(AtomNumber n)
        {
            AtomNumber result = new AtomNumber { type = n.type };
            // @formatter:off
            switch ( n.type )
            {
                case AtomNumberType.UINT_8:  result.val_uint8   =                       n.val_uint8;    break;
                case AtomNumberType.SINT_8:  result.val_sint8   =                       n.val_sint8;    break;
                case AtomNumberType.UINT16:  result.val_uint16  =                       n.val_uint16;   break;
                case AtomNumberType.SINT16:  result.val_sint16  =                       n.val_sint16;   break;
                case AtomNumberType._CHAR_:  result.val_char    =                       n.val_char;     break;
                case AtomNumberType.UINT32:  result.val_sint32  =                       n.val_sint32;   break;
                case AtomNumberType.SINT32:  result.val_uint32  =                       n.val_uint32;   break;
                case AtomNumberType.UINT64:  result.val_sint64  =                       n.val_sint64;   break;
                case AtomNumberType.SINT64:  result.val_uint64  =                       n.val_uint64;   break;
                case AtomNumberType.SINGLE:  result.val_single  = (float) Math.Truncate(n.ToSingle());  break;
                case AtomNumberType.DOUBLE:  result.val_double  =         Math.Truncate(n.ToDouble());  break;
                case AtomNumberType.DECIMAL: result.val_decimal =         Math.Truncate(n.ToDecimal()); break;
                case AtomNumberType.NaN:
                    if (!raiseNotANumber)
                        throw new InvalidOperationException("Can't apply operator to a not-number!");
                    break;
            }
            // @formatter:on
            return result;
        }

        public static AtomNumber Sqrt(AtomNumber n)
        {
            if (n.type == AtomNumberType.NaN)
                return n;
            return new AtomNumber
            {
                type       = AtomNumberType.DOUBLE,
                val_double = Math.Sqrt(n.ToDouble())
            };
        }

        public static AtomNumber Pow(AtomNumber a, AtomNumber b)
        {
            if (a.type == AtomNumberType.NaN)
                return a;
            if (b.type == AtomNumberType.NaN)
                return b;
            return new AtomNumber
            {
                type       = AtomNumberType.DOUBLE,
                val_double = Math.Pow(a.ToDouble(), b.ToDouble())
            };
        }

        public static AtomNumber Exp(AtomNumber n)
        {
            if (n.type == AtomNumberType.NaN)
                return n;
            return new AtomNumber
            {
                type       = AtomNumberType.DOUBLE,
                val_double = Math.Exp(n.ToDouble())
            };
        }

        public static AtomNumber Log(AtomNumber a, AtomNumber b)
        {
            if (a.type == AtomNumberType.NaN)
                return a;
            if (b.type == AtomNumberType.NaN)
                return b;
            return new AtomNumber
            {
                type       = AtomNumberType.DOUBLE,
                val_double = Math.Log(a.ToDouble(), b.ToDouble())
            };
        }

        public static AtomNumber Ln(AtomNumber n)
        {
            if (n.type == AtomNumberType.NaN)
                return n;
            return new AtomNumber
            {
                type       = AtomNumberType.DOUBLE,
                val_double = Math.Log(n.ToDouble())
            };
        }

        public static AtomNumber Log10(AtomNumber n)
        {
            if (n.type == AtomNumberType.NaN)
                return n;
            return new AtomNumber
            {
                type       = AtomNumberType.DOUBLE,
                val_double = Math.Log10(n.ToDouble())
            };
        }

        public static AtomNumber Sin(AtomNumber n)
        {
            if (n.type == AtomNumberType.NaN)
                return n;
            return new AtomNumber
            {
                type       = AtomNumberType.DOUBLE,
                val_double = Math.Sin(n.ToDouble())
            };
        }

        public static AtomNumber Cos(AtomNumber n)
        {
            if (n.type == AtomNumberType.NaN)
                return n;
            return new AtomNumber
            {
                type       = AtomNumberType.DOUBLE,
                val_double = Math.Cos(n.ToDouble())
            };
        }

        public static AtomNumber Tan(AtomNumber n)
        {
            if (n.type == AtomNumberType.NaN)
                return n;
            return new AtomNumber
            {
                type       = AtomNumberType.DOUBLE,
                val_double = Math.Tan(n.ToDouble())
            };
        }

        public static AtomNumber Asin(AtomNumber n)
        {
            if (n.type == AtomNumberType.NaN)
                return n;
            return new AtomNumber
            {
                type       = AtomNumberType.DOUBLE,
                val_double = Math.Asin(n.ToDouble())
            };
        }

        public static AtomNumber Acos(AtomNumber n)
        {
            if (n.type == AtomNumberType.NaN)
                return n;
            return new AtomNumber
            {
                type       = AtomNumberType.DOUBLE,
                val_double = Math.Acos(n.ToDouble())
            };
        }

        public static AtomNumber Atan(AtomNumber n)
        {
            if (n.type == AtomNumberType.NaN)
                return n;
            return new AtomNumber
            {
                type       = AtomNumberType.DOUBLE,
                val_double = Math.Atan(n.ToDouble())
            };
        }

        public static AtomNumber Atan2(AtomNumber a, AtomNumber b)
        {
            if (a.type == AtomNumberType.NaN)
                return a;
            if (b.type == AtomNumberType.NaN)
                return b;
            return new AtomNumber
            {
                type       = AtomNumberType.DOUBLE,
                val_double = Math.Atan2(a.ToDouble(), b.ToDouble())
            };
        }

        public static AtomNumber Sinh(AtomNumber n)
        {
            if (n.type == AtomNumberType.NaN)
                return n;
            return new AtomNumber
            {
                type       = AtomNumberType.DOUBLE,
                val_double = Math.Sinh(n.ToDouble())
            };
        }

        public static AtomNumber Cosh(AtomNumber n)
        {
            if (n.type == AtomNumberType.NaN)
                return n;
            return new AtomNumber
            {
                type       = AtomNumberType.DOUBLE,
                val_double = Math.Cosh(n.ToDouble())
            };
        }

        public static AtomNumber Tanh(AtomNumber n)
        {
            if (n.type == AtomNumberType.NaN)
                return n;
            return new AtomNumber
            {
                type       = AtomNumberType.DOUBLE,
                val_double = Math.Tanh(n.ToDouble())
            };
        }

#endregion

#region Binary operators

        public static AtomNumber And(AtomNumber a, AtomNumber b)
        {
            AtomNumber result = new AtomNumber { type = MaxNumberType(a, b) };
            // CS type operations upcast
            if (useCSUpTypeOperations &&
                result.type < AtomNumberType.SINT32)
                result.type = AtomNumberType.SINT32;
            // @formatter:off
            switch ( result.type )
            {
                case AtomNumberType.UINT_8: result.val_uint8    = (byte)    (a.ToUByte()   & b.ToUByte());   break;
                case AtomNumberType.SINT_8: result.val_sint8    = (sbyte)   (a.ToSByte()   & b.ToSByte());   break;
                case AtomNumberType.UINT16: result.val_uint16   = (ushort)  (a.ToUShort()  & b.ToUShort());  break;
                case AtomNumberType.SINT16: result.val_sint16   = (short)   (a.ToSShort()  & b.ToSShort());  break;
                case AtomNumberType._CHAR_: result.val_char     = (char)    (a.ToChar()    & b.ToChar());    break;
                case AtomNumberType.UINT32: result.val_uint32   = (uint)    (a.ToUInt()    & b.ToUInt());    break;
                case AtomNumberType.SINT32: result.val_sint32   = (int)     (a.ToSInt()    & b.ToSInt());    break;
                case AtomNumberType.UINT64: result.val_uint64   = (ulong)   (a.ToULong()   & b.ToULong());   break;
                case AtomNumberType.SINT64: result.val_sint64   = (long)    (a.ToSLong()   & b.ToSLong());   break;
                case AtomNumberType.SINGLE:
                case AtomNumberType.DOUBLE:
                case AtomNumberType.DECIMAL:
                    throw new InvalidOperationException("Operator & (AND) not applicable to floating-point numbers!");
                case AtomNumberType.NaN:
                    if (!raiseNotANumber)
                        throw new InvalidOperationException("Can't apply operator & (AND) to a not-number!");
                    break;
                
            }
            // @formatter:on
            return result;
        }

        public static AtomNumber Or(AtomNumber a, AtomNumber b)
        {
            AtomNumber result = new AtomNumber { type = MaxNumberType(a, b) };
            // CS type operations upcast
            if (useCSUpTypeOperations &&
                result.type < AtomNumberType.SINT32)
                result.type = AtomNumberType.SINT32;
            // @formatter:off
            switch ( result.type )
            {
                case AtomNumberType.UINT_8: result.val_uint8    = (byte)    (a.ToUByte()   | b.ToUByte());   break;
                case AtomNumberType.SINT_8: result.val_sint8    = (sbyte)   (a.ToSByte()   | b.ToSByte());   break;
                case AtomNumberType.UINT16: result.val_uint16   = (ushort)  (a.ToUShort()  | b.ToUShort());  break;
                case AtomNumberType.SINT16: result.val_sint16   = (short)   (a.ToSShort()  | b.ToSShort());  break;
                case AtomNumberType._CHAR_: result.val_char     = (char)    (a.ToChar()    | b.ToChar());    break;
                case AtomNumberType.UINT32: result.val_uint32   = (uint)    (a.ToUInt()    | b.ToUInt());    break;
                case AtomNumberType.SINT32: result.val_sint32   = (int)     (a.ToSInt()    | b.ToSInt());    break;
                case AtomNumberType.UINT64: result.val_uint64   = (ulong)   (a.ToULong()   | b.ToULong());   break;
                case AtomNumberType.SINT64: result.val_sint64   = (long)    (a.ToSLong()   | b.ToSLong());   break;
                case AtomNumberType.SINGLE:
                case AtomNumberType.DOUBLE:
                case AtomNumberType.DECIMAL:
                    throw new InvalidOperationException("Operator | (OR) not applicable to floating-point numbers!");
                case AtomNumberType.NaN:
                    if (!raiseNotANumber)
                        throw new InvalidOperationException("Can't apply operator | (OR) to a not-number!");
                    break;
                
            }
            // @formatter:on
            return result;
        }

        public static AtomNumber Xor(AtomNumber a, AtomNumber b)
        {
            AtomNumber result = new AtomNumber { type = MaxNumberType(a, b) };
            // CS type operations upcast
            if (useCSUpTypeOperations &&
                result.type < AtomNumberType.SINT32)
                result.type = AtomNumberType.SINT32;
            // @formatter:off
            switch ( result.type )
            {
                case AtomNumberType.UINT_8: result.val_uint8    = (byte)    (a.ToUByte()   ^ b.ToUByte());   break;
                case AtomNumberType.SINT_8: result.val_sint8    = (sbyte)   (a.ToSByte()   ^ b.ToSByte());   break;
                case AtomNumberType.UINT16: result.val_uint16   = (ushort)  (a.ToUShort()  ^ b.ToUShort());  break;
                case AtomNumberType.SINT16: result.val_sint16   = (short)   (a.ToSShort()  ^ b.ToSShort());  break;
                case AtomNumberType._CHAR_: result.val_char     = (char)    (a.ToChar()    ^ b.ToChar());    break;
                case AtomNumberType.UINT32: result.val_uint32   = (uint)    (a.ToUInt()    ^ b.ToUInt());    break;
                case AtomNumberType.SINT32: result.val_sint32   = (int)     (a.ToSInt()    ^ b.ToSInt());    break;
                case AtomNumberType.UINT64: result.val_uint64   = (ulong)   (a.ToULong()   ^ b.ToULong());   break;
                case AtomNumberType.SINT64: result.val_sint64   = (long)    (a.ToSLong()   ^ b.ToSLong());   break;
                case AtomNumberType.SINGLE:
                case AtomNumberType.DOUBLE:
                case AtomNumberType.DECIMAL:
                    throw new InvalidOperationException("Operator ^ (XOR) not applicable to floating-point numbers!");
                case AtomNumberType.NaN:
                    if (!raiseNotANumber)
                        throw new InvalidOperationException("Can't apply operator ^ (XOR) to a not-number!");
                    break;
                
            }
            // @formatter:on
            return result;
        }

        public static AtomNumber Lsh(AtomNumber a, AtomNumber b)
        {
            AtomNumber result = new AtomNumber { type = MaxNumberType(a, b) };
            // CS type operations upcast
            if (useCSUpTypeOperations &&
                result.type < AtomNumberType.SINT32)
                result.type = AtomNumberType.SINT32;
            // @formatter:off
            switch ( result.type )
            {
                case AtomNumberType.UINT_8: result.val_uint8    = (byte)    (a.ToUByte()   << b.ToUByte());   break;
                case AtomNumberType.SINT_8: result.val_sint8    = (sbyte)   (a.ToSByte()   << b.ToSByte());   break;
                case AtomNumberType.UINT16: result.val_uint16   = (ushort)  (a.ToUShort()  << b.ToUShort());  break;
                case AtomNumberType.SINT16: result.val_sint16   = (short)   (a.ToSShort()  << b.ToSShort());  break;
                case AtomNumberType._CHAR_: result.val_char     = (char)    (a.ToChar()    << b.ToChar());    break;
                case AtomNumberType.UINT32: result.val_uint32   = (uint)    (a.ToUInt()    << b.ToSInt());    break;
                case AtomNumberType.SINT32: result.val_sint32   = (int)     (a.ToSInt()    << b.ToSInt());    break;
                case AtomNumberType.UINT64: result.val_uint64   = (ulong)   (a.ToULong()   << b.ToSInt());   break;
                case AtomNumberType.SINT64: result.val_sint64   = (long)    (a.ToSLong()   << b.ToSInt());   break;
                case AtomNumberType.SINGLE:
                case AtomNumberType.DOUBLE:
                case AtomNumberType.DECIMAL:
                    throw new InvalidOperationException("Operator << (Left Shift) not applicable to floating-point numbers!");
                case AtomNumberType.NaN:
                    if (!raiseNotANumber)
                        throw new InvalidOperationException("Can't apply operator << (Left Shift) to a not-number!");
                    break;
                
            }
            // @formatter:on
            return result;
        }

        public static AtomNumber Rsh(AtomNumber a, AtomNumber b)
        {
            AtomNumber result = new AtomNumber { type = MaxNumberType(a, b) };
            // CS type operations upcast
            if (useCSUpTypeOperations &&
                result.type < AtomNumberType.SINT32)
                result.type = AtomNumberType.SINT32;
            // @formatter:off
            switch ( result.type )
            {
                case AtomNumberType.UINT_8: result.val_uint8    = (byte)    (a.ToUByte()   >> b.ToUByte());   break;
                case AtomNumberType.SINT_8: result.val_sint8    = (sbyte)   (a.ToSByte()   >> b.ToSByte());   break;
                case AtomNumberType.UINT16: result.val_uint16   = (ushort)  (a.ToUShort()  >> b.ToUShort());  break;
                case AtomNumberType.SINT16: result.val_sint16   = (short)   (a.ToSShort()  >> b.ToSShort());  break;
                case AtomNumberType._CHAR_: result.val_char     = (char)    (a.ToChar()    >> b.ToChar());    break;
                case AtomNumberType.UINT32: result.val_uint32   = (uint)    (a.ToUInt()    >> b.ToSInt());    break;
                case AtomNumberType.SINT32: result.val_sint32   = (int)     (a.ToSInt()    >> b.ToSInt());    break;
                case AtomNumberType.UINT64: result.val_uint64   = (ulong)   (a.ToULong()   >> b.ToSInt());   break;
                case AtomNumberType.SINT64: result.val_sint64   = (long)    (a.ToSLong()   >> b.ToSInt());   break;
                case AtomNumberType.SINGLE:
                case AtomNumberType.DOUBLE:
                case AtomNumberType.DECIMAL:
                    throw new InvalidOperationException("Operator >> (Right Shift) not applicable to floating-point numbers!");
                case AtomNumberType.NaN:
                    if (!raiseNotANumber)
                        throw new InvalidOperationException("Can't apply operator >> (Right Shift) to a not-number!");
                    break;
            }
            // @formatter:on
            return result;
        }

#endregion Binary operators

#region Logic operators

        public static bool Eq(AtomNumber a, AtomNumber b)
        {
            int type = MaxNumberType(a, b);
            // @formatter:off
            switch ( type )
            {
                case AtomNumberType.UINT_8:  return a.ToUByte()   == b.ToUByte();
                case AtomNumberType.SINT_8:  return a.ToSByte()   == b.ToSByte();
                case AtomNumberType.UINT16:  return a.ToUShort()  == b.ToUShort();
                case AtomNumberType.SINT16:  return a.ToSShort()  == b.ToSShort();
                case AtomNumberType._CHAR_:  return a.ToChar()    == b.ToChar();
                case AtomNumberType.UINT32:  return a.ToUInt()    == b.ToUInt();
                case AtomNumberType.SINT32:  return a.ToSInt()    == b.ToSInt();
                case AtomNumberType.UINT64:  return a.ToULong()   == b.ToULong();
                case AtomNumberType.SINT64:  return a.ToSLong()   == b.ToSLong();
                case AtomNumberType.SINGLE:  return Compare(a.ToSingle(), b.ToSingle());
                case AtomNumberType.DOUBLE:  return Compare(a.ToDouble(), b.ToDouble());
                case AtomNumberType.DECIMAL: return Compare(a.ToDecimal(), b.ToDecimal());
            }
            // @formatter:on
            throw new InvalidOperationException("Can't apply operator == to a not-numbers!");
        }

        public static bool Lt(AtomNumber a, AtomNumber b)
        {
            int type = MaxNumberType(a, b);
            // @formatter:off
            switch ( type )
            {
                case AtomNumberType.UINT_8:  return a.ToUByte()   < b.ToUByte();
                case AtomNumberType.SINT_8:  return a.ToSByte()   < b.ToSByte();
                case AtomNumberType.UINT16:  return a.ToUShort()  < b.ToUShort();
                case AtomNumberType.SINT16:  return a.ToSShort()  < b.ToSShort();
                case AtomNumberType._CHAR_:  return a.ToChar()    < b.ToChar();
                case AtomNumberType.UINT32:  return a.ToUInt()    < b.ToUInt();
                case AtomNumberType.SINT32:  return a.ToSInt()    < b.ToSInt();
                case AtomNumberType.UINT64:  return a.ToULong()   < b.ToULong();
                case AtomNumberType.SINT64:  return a.ToSLong()   < b.ToSLong();
                case AtomNumberType.SINGLE:  return a.ToSingle()  < b.ToSingle();
                case AtomNumberType.DOUBLE:  return a.ToDouble()  < b.ToDouble();
                case AtomNumberType.DECIMAL: return a.ToDecimal() < b.ToDecimal();
            }
            // @formatter:on
            throw new InvalidOperationException("Can't apply operator == to a not-numbers!");
        }

        public static bool Gt(AtomNumber a, AtomNumber b)
        {
            int type = MaxNumberType(a, b);
            // @formatter:off
            switch ( type )
            {
                case AtomNumberType.UINT_8:  return a.ToUByte()   > b.ToUByte();
                case AtomNumberType.SINT_8:  return a.ToSByte()   > b.ToSByte();
                case AtomNumberType.UINT16:  return a.ToUShort()  > b.ToUShort();
                case AtomNumberType.SINT16:  return a.ToSShort()  > b.ToSShort();
                case AtomNumberType._CHAR_:  return a.ToChar()    > b.ToChar();
                case AtomNumberType.UINT32:  return a.ToUInt()    > b.ToUInt();
                case AtomNumberType.SINT32:  return a.ToSInt()    > b.ToSInt();
                case AtomNumberType.UINT64:  return a.ToULong()   > b.ToULong();
                case AtomNumberType.SINT64:  return a.ToSLong()   > b.ToSLong();
                case AtomNumberType.SINGLE:  return a.ToSingle()  > b.ToSingle();
                case AtomNumberType.DOUBLE:  return a.ToDouble()  > b.ToDouble();
                case AtomNumberType.DECIMAL: return a.ToDecimal() > b.ToDecimal();
            }
            // @formatter:on
            throw new InvalidOperationException("Can't apply operator == to a not-numbers!");
        }

        public static bool Le(AtomNumber a, AtomNumber b)
        {
            int type = MaxNumberType(a, b);
            // @formatter:off
            switch ( type )
            {
                case AtomNumberType.UINT_8:  return a.ToUByte()   <= b.ToUByte();
                case AtomNumberType.SINT_8:  return a.ToSByte()   <= b.ToSByte();
                case AtomNumberType.UINT16:  return a.ToUShort()  <= b.ToUShort();
                case AtomNumberType.SINT16:  return a.ToSShort()  <= b.ToSShort();
                case AtomNumberType._CHAR_:  return a.ToChar()    <= b.ToChar();
                case AtomNumberType.UINT32:  return a.ToUInt()    <= b.ToUInt();
                case AtomNumberType.SINT32:  return a.ToSInt()    <= b.ToSInt();
                case AtomNumberType.UINT64:  return a.ToULong()   <= b.ToULong();
                case AtomNumberType.SINT64:  return a.ToSLong()   <= b.ToSLong();
                case AtomNumberType.SINGLE:  return a.ToSingle()  <= b.ToSingle();
                case AtomNumberType.DOUBLE:  return a.ToDouble()  <= b.ToDouble();
                case AtomNumberType.DECIMAL: return a.ToDecimal() <= b.ToDecimal();
            }
            // @formatter:on
            throw new InvalidOperationException("Can't apply operator == to a not-numbers!");
        }

        public static bool Ge(AtomNumber a, AtomNumber b)
        {
            int type = MaxNumberType(a, b);
            // @formatter:off
            switch ( type )
            {
                case AtomNumberType.UINT_8:  return a.ToUByte()   >= b.ToUByte();
                case AtomNumberType.SINT_8:  return a.ToSByte()   >= b.ToSByte();
                case AtomNumberType.UINT16:  return a.ToUShort()  >= b.ToUShort();
                case AtomNumberType.SINT16:  return a.ToSShort()  >= b.ToSShort();
                case AtomNumberType._CHAR_:  return a.ToChar()    >= b.ToChar();
                case AtomNumberType.UINT32:  return a.ToUInt()    >= b.ToUInt();
                case AtomNumberType.SINT32:  return a.ToSInt()    >= b.ToSInt();
                case AtomNumberType.UINT64:  return a.ToULong()   >= b.ToULong();
                case AtomNumberType.SINT64:  return a.ToSLong()   >= b.ToSLong();
                case AtomNumberType.SINGLE:  return a.ToSingle()  >= b.ToSingle();
                case AtomNumberType.DOUBLE:  return a.ToDouble()  >= b.ToDouble();
                case AtomNumberType.DECIMAL: return a.ToDecimal() >= b.ToDecimal();
            }
            // @formatter:on
            throw new InvalidOperationException("Can't apply operator == to a not-numbers!");
        }

        public static bool Ne(AtomNumber a, AtomNumber b)
        {
            int type = MaxNumberType(a, b);
            // @formatter:off
            switch ( type )
            {
                case AtomNumberType.UINT_8:  return a.ToUByte()   != b.ToUByte();
                case AtomNumberType.SINT_8:  return a.ToSByte()   != b.ToSByte();
                case AtomNumberType.UINT16:  return a.ToUShort()  != b.ToUShort();
                case AtomNumberType.SINT16:  return a.ToSShort()  != b.ToSShort();
                case AtomNumberType._CHAR_:  return a.ToChar()    != b.ToChar();
                case AtomNumberType.UINT32:  return a.ToUInt()    != b.ToUInt();
                case AtomNumberType.SINT32:  return a.ToSInt()    != b.ToSInt();
                case AtomNumberType.UINT64:  return a.ToULong()   != b.ToULong();
                case AtomNumberType.SINT64:  return a.ToSLong()   != b.ToSLong();
                case AtomNumberType.SINGLE:  return !Compare(a.ToSingle(), b.ToSingle());
                case AtomNumberType.DOUBLE:  return !Compare(a.ToDouble(), b.ToDouble());
                case AtomNumberType.DECIMAL: return !Compare(a.ToDecimal(), b.ToDecimal());
            }
            // @formatter:on
            throw new InvalidOperationException("Can't apply operator == to a not-numbers!");
        }

#endregion Logic operators
    }
}