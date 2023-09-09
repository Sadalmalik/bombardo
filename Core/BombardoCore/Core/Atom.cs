using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Bombardo.Core
{
    public struct AtomPair
    {
        public Atom atom;
        public Atom next;

        public bool IsEmpty => atom == null && next == null;
    }

    //[StructLayout(LayoutKind.Explicit, Pack = 1)]
    public class Atom
    {
        public static readonly string EMPTY_PAIR = "( )";

        //[FieldOffset(0)]
        public int @type;

        //[FieldOffset(sizeof(int))]
        public AtomPair   @pair;
        public string     @string;
        public bool       @bool;
        public AtomNumber @number;
        public Function   @function;
        public Context    @context;
        public object     @object;

        public Atom Head
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => @pair.atom;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => @pair.atom = value;
        }

        public Atom Next
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => @pair.next;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => @pair.next = value;
        }

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => type == AtomType.Pair && pair.IsEmpty;
        }

        public bool IsPair
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => type == AtomType.Pair;
        }

        public bool IsSymbol
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => type == AtomType.Symbol;
        }

        public bool IsString
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => type == AtomType.String;
        }

        public bool IsBool
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => type == AtomType.Bool;
        }

        public bool IsNumber
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => type == AtomType.Number;
        }

        public bool IsFunction
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => type == AtomType.Function;
        }

        public bool IsContext
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => type == AtomType.Context;
        }

        public bool IsNative
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => type == AtomType.Native;
        }

        public Atom(int type)
        {
            this.@type = type;
            @pair.atom = null;
            @pair.next = null;
        }

        public static Atom CreatePair(Atom value, Atom next)
        {
            var atom = new Atom(AtomType.Pair);
            atom.pair.atom = value;
            atom.pair.next = next;
            return atom;
        }

        public static Atom CreateSymbol(string symbol)
        {
            var atom = new Atom(AtomType.Symbol);
            atom.@string = symbol;
            return atom;
        }

        public static Atom CreateString(string @string)
        {
            var atom = new Atom(AtomType.String);
            atom.@string = @string;
            return atom;
        }

        public static Atom CreateBoolean(bool boolean)
        {
            var atom = new Atom(AtomType.Bool);
            atom.@bool = boolean;
            return atom;
        }

        public static Atom CreateNumber(AtomNumber number)
        {
            var atom = new Atom(AtomType.Number);
            atom.@number = number;
            return atom;
        }

        public static Atom CreateNumber(byte value)
        {
            return CreateNumber(new AtomNumber
            {
                type      = AtomNumberType.UINT_8,
                val_uint8 = value
            });
        }

        public static Atom CreateNumber(sbyte value)
        {
            return CreateNumber(new AtomNumber
            {
                type      = AtomNumberType.SINT_8,
                val_sint8 = value
            });
        }

        public static Atom CreateNumber(ushort value)
        {
            return CreateNumber(new AtomNumber
            {
                type       = AtomNumberType.UINT16,
                val_uint16 = value
            });
        }

        public static Atom CreateNumber(short value)
        {
            return CreateNumber(new AtomNumber
            {
                type       = AtomNumberType.SINT16,
                val_sint16 = value
            });
        }

        public static Atom CreateNumber(char value)
        {
            return CreateNumber(new AtomNumber
            {
                type     = AtomNumberType._CHAR_,
                val_char = value
            });
        }

        public static Atom CreateNumber(uint value)
        {
            return CreateNumber(new AtomNumber
            {
                type       = AtomNumberType.UINT32,
                val_uint32 = value
            });
        }

        public static Atom CreateNumber(int value)
        {
            return CreateNumber(new AtomNumber
            {
                type       = AtomNumberType.SINT32,
                val_sint32 = value
            });
        }

        public static Atom CreateNumber(ulong value)
        {
            return CreateNumber(new AtomNumber
            {
                type       = AtomNumberType.UINT64,
                val_uint64 = value
            });
        }

        public static Atom CreateNumber(long value)
        {
            return CreateNumber(new AtomNumber
            {
                type       = AtomNumberType.SINT64,
                val_sint64 = value
            });
        }

        public static Atom CreateNumber(float value)
        {
            return CreateNumber(new AtomNumber
            {
                type       = AtomNumberType.SINGLE,
                val_single = value
            });
        }

        public static Atom CreateNumber(double value)
        {
            return CreateNumber(new AtomNumber
            {
                type       = AtomNumberType.DOUBLE,
                val_double = value
            });
        }

        public static Atom CreateNumber(decimal value)
        {
            return CreateNumber(new AtomNumber
            {
                type        = AtomNumberType.DECIMAL,
                val_decimal = value
            });
        }

        public static Atom CreateFunction(Function function)
        {
            var atom = new Atom(AtomType.Function);
            atom.@function = function;
            return atom;
        }

        public static Atom CreateContext(Context context)
        {
            var atom = new Atom(AtomType.Context);
            atom.@context = context;
            return atom;
        }

        public static Atom CreateNative(object native)
        {
            var atom = new Atom(AtomType.Native);
            atom.@object = native;
            return atom;
        }

        public int ListLength()
        {
            if (type != AtomType.Pair) return -1;
            int count = 0;
            for (Atom item = this; item != null; item = item.pair.next) count++;
            return count;
        }

        public Atom ListSkip(int count)
        {
            Atom atom = this;
            while (count > 0 && atom != null)
            {
                atom = atom.pair.next;
                count--;
            }

            return atom;
        }

        public string Stringify(bool recursive = true, bool wrapped = false)
        {
            switch (type)
            {
                case AtomType.Symbol: return @string;
                case AtomType.String: return $"\"{@string}\"";
                case AtomType.Bool:   return @bool ? "true" : "false";
                case AtomType.Number: return @number.ToString();
                case AtomType.Pair:
                {
                    if (@pair.atom == null &&
                        @pair.next == null)
                        return Atom.EMPTY_PAIR;

                    StringBuilder sb = new StringBuilder();
                    if (!wrapped) sb.Append("( ");

                    Atom atom = @pair.atom;
                    if (atom == null)
                    {
                        sb.Append(Names.NULL_SYMBOL);
                    }
                    else if (recursive || atom.type != AtomType.Pair)
                    {
                        sb.Append(atom.Stringify(recursive));
                    }
                    else
                    {
                        sb.Append("<list>");
                    }

                    Atom next = @pair.next;
                    if (next != null)
                    {
                        sb.Append(" ");
                        if (next.type == AtomType.Pair)
                        {
                            sb.Append(next.Stringify(recursive, true));
                        }
                        else
                        {
                            sb.Append(". ");
                            sb.Append(next.Stringify(recursive, true));
                        }
                    }

                    if (!wrapped) sb.Append(" )");

                    return sb.ToString();
                }

                case AtomType.Function:
                    return $"<procedure {function.Name}>";

                case AtomType.Native:
                    return $"<object {@object}>";

                default:
                    return "undefined";
            }
        }

        public override string ToString()
        {
            return Stringify(false);
        }
    }
}