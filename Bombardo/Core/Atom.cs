using System;
using System.Text;

namespace Bombardo
{
    public class AtomType
    {
        public const int Undefined = -1;
        public const int Pair = 0;
        public const int Symbol = 1;
        public const int String = 2;
        public const int Bool = 3;
        public const int Number = 4;
        public const int Procedure = 5;
        public const int Native = 6;

        public static string ToString(int type)
        {
            switch (type)
            {
                case Pair: return "List";
                case Symbol: return "Symbol";
                case String: return "String";
                case Bool: return "Bool";
                case Number: return "Number";
                case Procedure: return "Procedure";
                case Native: return "Native";
                default: return "Undefined";
            }
        }
    }

    public class Atom
    {
        public static readonly string EMPTY_PAIR = "()";

        public static readonly Atom EMPTY = new Atom();
        public static readonly Atom TRUE = new Atom(AtomType.Bool, true);
        public static readonly Atom FALSE = new Atom(AtomType.Bool, false);
        public static readonly Atom QUOTE = new Atom(AtomType.Symbol, "quote");

        public int type;
        public object value;
        public Atom atom { get { return value as Atom; } }
        public Atom next;

        public Atom()
        {
            this.type = AtomType.Pair;
            this.value = null;
            this.next = null;
        }

        public Atom(string value)
        {
            this.type = AtomType.Symbol;
            this.value = value;
            this.next = null;
        }

        public Atom(int type, object value = null)
        {
            this.type = type;
            this.value = value;
            this.next = null;
        }

        public Atom(Atom value, Atom next)
        {
            this.type = AtomType.Pair;
            this.value = value;
            this.next = next;
        }

        public Atom(StringBuilder sb)
        {
            this.type = AtomType.String;
            this.value = sb.ToString();
            this.next = null;
        }

        public bool IsEmpty() { return type == AtomType.Pair && value==null && next==null; }
        public bool IsPair() { return type == AtomType.Pair; }
        public bool IsSymbol() { return type == AtomType.Symbol; }
        public bool IsString() { return type == AtomType.String; }
        public bool IsBool() { return type == AtomType.Bool; }
        public bool IsNumber() { return type == AtomType.Number; }
        public bool IsProcedure() { return type == AtomType.Procedure; }
        public bool IsNative() { return type == AtomType.Native; }
        
        public int ListLength()
        {
            if (type != AtomType.Pair) return -1;
            int count = 0;
            for (Atom item = this; item != null; item = item.next) count++;
            return count;
        }

        public Atom ListSkip(int count)
        {
            Atom atom = this;
            while (count > 0 && atom != null)
            {
                atom = atom.next;
                count--;
            }
            return atom;
        }

        public string Stringify(bool recursive = true, bool wrapped=false)
        {
            switch (type)
            {
                case AtomType.Symbol: return (string)value;
                case AtomType.String: return string.Format("\"{0}\"", value);
                case AtomType.Bool: return (bool)value ? "#T" : "#F";
                case AtomType.Number: return value.ToString();
                case AtomType.Pair:
                    {
                        if (this.value == null && this.next == null)
                            return Atom.EMPTY_PAIR;
                        StringBuilder sb = new StringBuilder();
                        if (!wrapped) sb.Append("( ");

                        Atom value = (Atom)this.value;
                        if (value == null)
                        {
                            sb.Append(AllNames.NULL_SYMBOL);
                        }
                        else if (recursive || value.type != AtomType.Pair)
                        {
                            sb.Append(value.Stringify(recursive));
                        }
                        else
                        {
                            sb.Append("<list>");
                        }
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

                case AtomType.Procedure:
                    return string.Format("<procedure {0}>", ((Procedure)value).Name);

                case AtomType.Native:
                    return string.Format("<object {0}>", value.ToString());

                default:
                    return "undefined";
            }
        }

        public override string ToString()
        {
            return Stringify(true);
        }

        public static bool Compare(Atom a, Atom b)
        {
            bool aNull = a == null;
            bool bNull = b == null;
            if (aNull && bNull)
                return true;
            if ((aNull && !bNull) || (!aNull && bNull))
                return false;
            if (a.type != b.type)
                return false;
            if (a.type == AtomType.Pair)
                return Compare((Atom)a.value, (Atom)b.value) && Compare(a.next, b.next);
            return a.value.Equals(b.value);
        }

        public static void Each(Atom list, Action<Atom> callback)
        {
            if (list.type != AtomType.Pair)
                throw new ArgumentException("Atom must be list!");
            if (callback == null)
                throw new ArgumentException("Callback can't be null!");
            for (Atom iter = list; iter != null; iter = iter.next)
                callback((Atom)iter.value);
        }

        public static Atom List(params Atom[] atoms)
        {
            Atom list = null, tail = null;
            for (int i = 0; i < atoms.Length; i++)
            {
                if (tail == null)
                    tail = list = new Atom();
                else tail = tail.next = new Atom();
                tail.value = atoms[i];
            }
            return list;
        }

        public static Atom CloneList(Atom atom)
        {
            if (atom == null) return null;
            switch (atom.type)
            {
                case AtomType.Pair:
                    return new Atom(
                        atom.atom,
                        CloneList(atom.next));

                default:
                    return atom;
            }
        }

        public static Atom CloneTree(Atom atom)
        {
            if (atom == null) return null;
            switch (atom.type)
            {
                case AtomType.Pair:
                    return new Atom(
                        CloneTree((Atom)atom.value),
                        CloneTree(atom.next));

                default:
                    return atom;
            }
        }
    }
}